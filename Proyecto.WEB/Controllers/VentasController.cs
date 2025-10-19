using Microsoft.AspNetCore.Mvc;
using Proyecto.BLL.Interfaces;
using Proyecto.MODELS;
using Proyecto.WEB.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.WEB.Controllers
{
    public class VentasController : Controller
    {
        private readonly IVentasService _ventasService;
        private readonly IProductoService _productoService;

        public VentasController(IVentasService ventasService, IProductoService productoService)
        {
            _ventasService = ventasService;
            _productoService = productoService;
        }

        // =========================
        // Listar todas las ventas
        // =========================
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

        // =========================
        // Detalle de venta
        // =========================
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var venta = await _ventasService.Obtener(id);
            if (venta == null)
            {
                TempData["Error"] = "La venta solicitada no existe.";
                return RedirectToAction(nameof(Index));
            }

            var ventaVM = new VentaViewModel
            {
                IdVenta = venta.IdVenta,
                Fecha = venta.Fecha,
                Total = venta.Total,
                IdCliente = venta.IdCliente,
                Pagado = venta.Pagado,
                EsFiado = venta.EsFiado,
                Anulado = venta.Anulado,
                DetalleVenta = venta.DetalleVenta.Select(d => new DetalleVentaViewModel
                {
                    IdDetalle = d.IdDetalle,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal ?? 0
                }).ToList()
            };

            return View(ventaVM);
        }

        // =========================
        // Formulario para crear venta
        // =========================
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

        // =========================
        // Guardar venta usando la lógica de negocio
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(VentaViewModel ventaVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos. Revisa la información de la venta.";
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
             
                var venta = new Ventum
                {
                    IdUsuario = 20, // <-- clave foránea correcta
                    IdCliente = ventaVM.IdCliente,
                    EsFiado = ventaVM.EsFiado,
                    Pagado = ventaVM.Pagado,
                    Observaciones = ventaVM.Observaciones,
                    DetalleVenta = ventaVM.DetalleVenta.Select(d => new DetalleVentum
                    {
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad
                    }).ToList()
                };

                var ventaGuardada = await _ventasService.GuardarVenta(venta);

                TempData["Exito"] = "Venta registrada correctamente.";
                return RedirectToAction("Detalle", new { id = ventaGuardada.IdVenta });
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = $"Error al registrar la venta: {ex.Message}";
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
        }


        // =========================
        // Anular venta usando lógica de negocio
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(int id)
        {
            bool exito = await _ventasService.AnularVenta(id);
            TempData[exito ? "Exito" : "Error"] = exito
                ? "Venta anulada correctamente."
                : "No se pudo anular la venta.";

            return RedirectToAction(nameof(Index));
        }
    }
}
