using Proyecto.BLL.Interfaces;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.BLL.Servicios
{
    public class VentasService : IVentasService
    {
        private readonly IGenericRepository<Ventum> _ventaRepositorio;
        private readonly IGenericRepository<Producto> _productoRepositorio;

        public VentasService(IGenericRepository<Ventum> ventaRepositorio,
                             IGenericRepository<Producto> productoRepositorio)
        {
            _ventaRepositorio = ventaRepositorio;
            _productoRepositorio = productoRepositorio;
        }

        // Guardar venta
        public async Task<Ventum> GuardarVenta(Ventum venta)
        {
            foreach (var detalle in venta.DetalleVenta)
            {
                var producto = await _productoRepositorio.obtener(detalle.IdProducto);
                if (producto == null)
                    throw new Exception($"Producto {detalle.IdProducto} no encontrado.");

                if (producto.Stock < detalle.Cantidad)
                    throw new Exception($"No hay suficiente stock de {producto.Nombre}.");

                detalle.PrecioUnitario = producto.PrecioVenta;
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;

                // Actualizar stock
                producto.Stock -= detalle.Cantidad;
                await _productoRepositorio.Actualizar(producto);
            }

            venta.Total = venta.DetalleVenta.Sum(d => d.Subtotal ?? 0);
            venta.Fecha = DateTime.Now;

            await _ventaRepositorio.Crear(venta);

            return venta;
        }

        // Obtener venta simple
        public async Task<Ventum?> Obtener(int id)
        {
            return await _ventaRepositorio.obtener(id);
        }

        // Obtener venta con detalles y productos
        public async Task<Ventum?> ObtenerConDetalle(int id)
        {
            // Usamos IQueryable desde el repositorio
            var query = await _ventaRepositorio.obtenerTodos(); // IQueryable<Ventum>

            return await query
                .Include(v => v.DetalleVenta)
                    .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(v => v.IdVenta == id);
        }

        // Listar todas las ventas
        public async Task<IQueryable<Ventum>> ObtenerTodos()
        {
            return await _ventaRepositorio.obtenerTodos();
        }

        public Task<decimal> CalcularTotalVenta(Ventum venta)
        {
            decimal total = venta.DetalleVenta.Sum(d => (d.Subtotal ?? (d.Cantidad * d.PrecioUnitario)));
            return Task.FromResult(total);
        }

        public async Task<bool> ActualizarStockDespuesDeVenta(Ventum venta)
        {
            foreach (var detalle in venta.DetalleVenta)
            {
                var producto = await _productoRepositorio.obtener(detalle.IdProducto);
                if (producto == null) return false;

                producto.Stock -= detalle.Cantidad;
                await _productoRepositorio.Actualizar(producto);
            }
            return true;
        }

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
                }
            }
            return true;
        }

        public async Task<bool> EliminarProductoDeVenta(int idVenta, int idDetalle)
        {
            var venta = await _ventaRepositorio.obtener(idVenta);
            if (venta == null) return false;

            var detalle = venta.DetalleVenta.FirstOrDefault(d => d.IdDetalle == idDetalle);
            if (detalle == null) return false;

            venta.DetalleVenta.Remove(detalle);
            await _ventaRepositorio.Actualizar(venta);

            return true;
        }
    }
}
