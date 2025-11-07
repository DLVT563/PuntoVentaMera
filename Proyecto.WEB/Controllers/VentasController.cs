using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BLL.Interfaces;
using Proyecto.MODELS;
using Proyecto.WEB.Models.ViewModels;

namespace Proyecto.WEB.Controllers
{

     public class VentasController : BaseController
    {
        private readonly IVentasService _ventasService;
        private readonly IProductoService _productoService;

        public VentasController(IVentasService ventasService, IProductoService productoService)
        {
            _ventasService = ventasService;
            _productoService = productoService;
        }
        [Authorize(Roles = "Administrador,Vendedor")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ventas = await _ventasService.ObtenerTodos();

            var ventasVM = ventas.Select(v => new VentaViewModel
            {
                IdVenta = v.IdVenta,
                Fecha = v.Fecha,
                Total = v.Total,
                IdCliente = v.IdCliente,
                Pagado = v.Pagado,
                EsFiado = v.EsFiado,
                Anulado = v.Anulado
            }).ToList();

            return View(ventasVM);
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var venta = await _ventasService.ObtenerConDetalle(id);
            if (venta == null)
                return RedirectToAction(nameof(Index));

            var historialStock = await _ventasService.ObtenerHistorialStockPorVenta(id);

            var ventaVM = new VentaViewModel
            {
                IdVenta = venta.IdVenta,
                Fecha = venta.Fecha,
                Total = venta.Total,
                IdCliente = venta.IdCliente,
                Pagado = venta.Pagado,
                EsFiado = venta.EsFiado,
                Anulado = venta.Anulado,
                NumeroVenta = venta.NumeroVenta,
                TipoComprobante = venta.TipoComprobante,
                Serie = venta.Serie,
                NumeroDocumento = venta.NumeroDocumento,
                MetodoPago = venta.MetodoPago,
                Observaciones = venta.Observaciones,
                SaldoPendiente = venta.SaldoPendiente,
                FechaPagoEstimada = venta.FechaPagoEstimada,
                DetalleVenta = venta.DetalleVenta.Select(d => new DetalleVentaViewModel
                {
                    IdDetalle = d.IdDetalle,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal ?? 0
                }).ToList(),
                HistorialStock = historialStock.Select(m => new MovimientoStockViewModel
                {
                    IdMovimiento = m.IdMovimiento,
                    IdProducto = m.IdProducto,
                    NombreProducto = m.IdProductoNavigation.Nombre,
                    Cantidad = m.Cantidad,
                    Fecha = m.Fecha ?? DateTime.Now,
                    Tipo = m.Tipo ?? "",
                    Referencia = m.Referencia ?? "",
                    Motivo = m.Motivo ?? "",
                    IdUsuario = m.IdUsuario,
                    NombreUsuario = m.IdUsuarioNavigation?.Nombre ?? ""
                }).ToList()
            };

            return View(ventaVM);
        }
        [Authorize(Roles = "Administrador,Vendedor, Cajero")]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var productos = await _productoService.obtenerTodos();
            var productosVM = productos.Select(p => new ProductoViewModel
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                PrecioVenta = p.PrecioVenta,
                Stock = p.Stock
            }).ToList();

            var ventaVM = new VentaViewModel
            {
                ProductosDisponibles = productosVM
            };

            return View(ventaVM);
        }

        [Authorize(Roles = "Administrador,Vendedor, Cajero")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(VentaViewModel ventaVM, string DetalleVentaJson)
        {
            // Parsear los productos agregados
            if (string.IsNullOrEmpty(DetalleVentaJson))
            {
                ModelState.AddModelError("", "Debe agregar al menos un producto a la venta.");
                var productos = await _productoService.obtenerTodos();
                ventaVM.ProductosDisponibles = productos.Select(p => new ProductoViewModel
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    PrecioVenta = p.PrecioVenta,
                    Stock = p.Stock
                }).ToList();
                return View(ventaVM);
            }

            ventaVM.DetalleVenta = System.Text.Json.JsonSerializer.Deserialize<List<DetalleVentaViewModel>>(DetalleVentaJson);

            if (!ModelState.IsValid || ventaVM.DetalleVenta == null || !ventaVM.DetalleVenta.Any())
            {
                var productos = await _productoService.obtenerTodos();
                ventaVM.ProductosDisponibles = productos.Select(p => new ProductoViewModel
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    PrecioVenta = p.PrecioVenta,
                    Stock = p.Stock
                }).ToList();
                return View(ventaVM);
            }

            try
            {
                // Generar valores automáticos
                var numeroVenta = $"V-{DateTime.Now:yyyyMMddHHmmss}";
                var serie = $"A001-{DateTime.Now:ddMMyyyy-HHmmss}";
                var numeroDocumento = ventaVM.NumeroDocumento;

                var venta = new Ventum
                {
                    IdUsuario = int.Parse(User.FindFirst("IdUsuario")!.Value),
                    IdCliente = ventaVM.IdCliente,
                    EsFiado = ventaVM.EsFiado,
                    Pagado = ventaVM.Pagado,
                    Observaciones = ventaVM.Observaciones,
                    NumeroVenta = numeroVenta,
                    TipoComprobante = ventaVM.TipoComprobante ?? "Factura",
                    Serie = serie,
                    NumeroDocumento = numeroDocumento,
                    MetodoPago = ventaVM.MetodoPago ?? "Efectivo",
                    Fecha = DateTime.Now,
                    FechaPagoEstimada = ventaVM.FechaPagoEstimada,
                    SaldoPendiente = ventaVM.EsFiado ? ventaVM.Total : 0,

                    DetalleVenta = ventaVM.DetalleVenta.Select(d => new DetalleVentum
                    {
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad
                    }).ToList()
                };

                var ventaGuardada = await _ventasService.GuardarVenta(venta);

                return RedirectToAction("Ventas", "Crear");
            }
            catch (Exception ex)
            {
                var productos = await _productoService.obtenerTodos();
                ventaVM.ProductosDisponibles = productos.Select(p => new ProductoViewModel
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    PrecioVenta = p.PrecioVenta,
                    Stock = p.Stock
                }).ToList();
                ModelState.AddModelError("", $"Error al registrar la venta: {ex.Message}");
                return View(ventaVM);
            }
        }
        [Authorize(Roles = "Administrador,Vendedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(int id)
        {
            bool exito = await _ventasService.AnularVenta(id);
            SetNotification(
                exito ? "Venta anulada correctamente." : "No se pudo anular la venta.",
                exito ? NotificationType.Success : NotificationType.Error);

            return RedirectToAction(nameof(Index));
        }
    }
}
