using Microsoft.EntityFrameworkCore;
using Proyecto.BLL.Interfaces;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.BLL.Servicios
{
    public class VentasService : IVentasService
    {
        private readonly IGenericRepository<Ventum> _ventaRepositorio;
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IGenericRepository<MovimientoStock> _movimientoRepositorio;

        public VentasService(
            IGenericRepository<Ventum> ventaRepositorio,
            IGenericRepository<Producto> productoRepositorio,
            IGenericRepository<MovimientoStock> movimientoRepositorio)
        {
            _ventaRepositorio = ventaRepositorio;
            _productoRepositorio = productoRepositorio;
            _movimientoRepositorio = movimientoRepositorio;
        }

        // =========================
        // Guardar venta y actualizar stock
        // =========================
        public async Task<Ventum> GuardarVenta(Ventum venta)
        {
            if (venta.DetalleVenta == null || !venta.DetalleVenta.Any())
                throw new Exception("Debe agregar al menos un producto a la venta.");

            // Asignar precios y subtotal
            foreach (var detalle in venta.DetalleVenta)
            {
                var producto = await _productoRepositorio.obtener(detalle.IdProducto);
                if (producto == null)
                    throw new Exception($"Producto {detalle.IdProducto} no encontrado.");

                if (producto.Stock < detalle.Cantidad)
                    throw new Exception($"No hay suficiente stock de {producto.Nombre}.");

                detalle.PrecioUnitario = producto.PrecioVenta;
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
            }

            // Calcular total y fecha
            venta.Total = venta.DetalleVenta.Sum(d => d.Subtotal ?? 0);
            venta.Fecha = DateTime.Now;

            // Guardar venta primero para generar IdVenta
            await _ventaRepositorio.Crear(venta);

            // Actualizar stock y registrar movimientos
            foreach (var detalle in venta.DetalleVenta)
            {
                var producto = await _productoRepositorio.obtener(detalle.IdProducto);
                producto.Stock -= detalle.Cantidad;
                await _productoRepositorio.Actualizar(producto);

                await _movimientoRepositorio.Crear(new MovimientoStock
                {
                    IdProducto = producto.IdProducto,
                    Cantidad = -detalle.Cantidad,
                    Fecha = DateTime.Now,
                    Tipo = "SALIDA", // ahora cumple con el CHECK de la base
                    Motivo = $"Venta {venta.IdVenta}",
                    Referencia = $"VENTA-{venta.IdVenta}"
                });
            }

            return venta;
        }

        // =========================
        // Obtener venta simple
        // =========================
        public async Task<Ventum?> Obtener(int id)
        {
            return await _ventaRepositorio.obtener(id);
        }

        // =========================
        // Obtener venta con detalles y productos
        // =========================
        public async Task<Ventum?> ObtenerConDetalle(int id)
        {
            var query = await _ventaRepositorio.obtenerTodos();
            return await query
                .Include(v => v.DetalleVenta)
                    .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(v => v.IdVenta == id);
        }

        // =========================
        // Listar todas las ventas
        // =========================
        public async Task<IQueryable<Ventum>> ObtenerTodos()
        {
            return await _ventaRepositorio.obtenerTodos();
        }

        // =========================
        // Calcular total de la venta
        // =========================
        public Task<decimal> CalcularTotalVenta(Ventum venta)
        {
            decimal total = venta.DetalleVenta.Sum(d => (d.Subtotal ?? (d.Cantidad * d.PrecioUnitario)));
            return Task.FromResult(total);
        }

        // =========================
        // Actualizar stock después de venta (para usar en otros procesos)
        // =========================
        public async Task<bool> ActualizarStockDespuesDeVenta(Ventum venta)
        {
            foreach (var detalle in venta.DetalleVenta)
            {
                var producto = await _productoRepositorio.obtener(detalle.IdProducto);
                if (producto == null) return false;

                producto.Stock -= detalle.Cantidad;
                await _productoRepositorio.Actualizar(producto);

                await _movimientoRepositorio.Crear(new MovimientoStock
                {
                    IdProducto = producto.IdProducto,
                    Cantidad = -detalle.Cantidad,
                    Fecha = DateTime.Now,
                    Tipo = "SALIDA",
                    Motivo = $"Venta {venta.IdVenta}",
                    Referencia = $"VENTA-{venta.IdVenta}"
                });
            }
            return true;
        }

        // =========================
        // Anular venta y devolver stock
        // =========================
        public async Task<bool> AnularVenta(int idVenta)
        {
            var venta = await _ventaRepositorio.obtener(idVenta);
            if (venta == null) return false;

            venta.Anulado = true;
            venta.FechaAnulacion = DateTime.Now;
            await _ventaRepositorio.Actualizar(venta);

            foreach (var detalle in venta.DetalleVenta)
            {
                var producto = await _productoRepositorio.obtener(detalle.IdProducto);
                if (producto != null)
                {
                    producto.Stock += detalle.Cantidad;
                    await _productoRepositorio.Actualizar(producto);

                    await _movimientoRepositorio.Crear(new MovimientoStock
                    {
                        IdProducto = producto.IdProducto,
                        Cantidad = detalle.Cantidad,
                        Fecha = DateTime.Now,
                        Tipo = "ENTRADA",
                        Motivo = $"Anulación Venta {venta.IdVenta}",
                        Referencia = $"VENTA-{venta.IdVenta}"
                    });
                }
            }
            return true;
        }

        // =========================
        // Eliminar un producto de la venta y devolver stock
        // =========================
        public async Task<bool> EliminarProductoDeVenta(int idVenta, int idDetalle)
        {
            var venta = await _ventaRepositorio.obtener(idVenta);
            if (venta == null) return false;

            var detalle = venta.DetalleVenta.FirstOrDefault(d => d.IdDetalle == idDetalle);
            if (detalle == null) return false;

            var producto = await _productoRepositorio.obtener(detalle.IdProducto);
            if (producto != null)
            {
                producto.Stock += detalle.Cantidad;
                await _productoRepositorio.Actualizar(producto);

                await _movimientoRepositorio.Crear(new MovimientoStock
                {
                    IdProducto = producto.IdProducto,
                    Cantidad = detalle.Cantidad,
                    Fecha = DateTime.Now,
                    Tipo = "ENTRADA",
                    Motivo = $"Eliminación Producto de Venta {venta.IdVenta}",
                    Referencia = $"VENTA-{venta.IdVenta}"
                });
            }

            venta.DetalleVenta.Remove(detalle);
            await _ventaRepositorio.Actualizar(venta);

            return true;
        }

        // =========================
        // Obtener historial de stock por venta (una sola consulta)
        // =========================
        public async Task<List<MovimientoStock>> ObtenerHistorialStockPorVenta(int idVenta)
        {
            var venta = await _ventaRepositorio.obtener(idVenta);
            if (venta == null) return new List<MovimientoStock>();

            var productosIds = venta.DetalleVenta.Select(d => d.IdProducto).ToList();
            var movimientosQuery = await _movimientoRepositorio.obtenerTodos(); // IQueryable<MovimientoStock>

            var historial = await movimientosQuery
                .Where(m => productosIds.Contains(m.IdProducto) &&
                            m.Referencia == $"VENTA-{venta.IdVenta}")
                .Include(m => m.IdProductoNavigation)
                .Include(m => m.IdUsuarioNavigation)
                .ToListAsync();

            return historial;
        }
    }
}
