using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BLL.Interfaces;
using Proyecto.MODELS;
using Proyecto.WEB.Models;
using Proyecto.WEB.Models.ViewModels;

namespace Proyecto.WEB.Controllers
{
    [Authorize(Roles = "Administrador, Vendedor")]
    public class ProductosController : BaseController
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.obtenerTodos();

            var productosVM = productos.Select(p => new ProductoViewModel
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioCompra = p.PrecioCompra,
                PrecioVenta = p.PrecioVenta,
                Stock = p.Stock,
                IdCategoria = p.IdCategoria,
                NombreCategoria = p.IdCategoriaNavigation.Nombre,
                Activo = p.Activo ?? false,
                CodigoBarras = p.CodigoBarras
            }).ToList();

            return View(productosVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var producto = await _productoService.obtener(id);
            if (producto == null)
            {
                SetNotification("El producto solicitado no existe.", NotificationType.Error);
                return RedirectToAction(nameof(Index));
            }

            var productoVM = new ProductoViewModel
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                PrecioCompra = producto.PrecioCompra,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                IdCategoria = producto.IdCategoria,
                NombreCategoria = producto.IdCategoriaNavigation?.Nombre,
                Activo = producto.Activo ?? false,
                CodigoBarras = producto.CodigoBarras
            };

            return View(productoVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel productoVM)
        {
            if (!ModelState.IsValid)
            {
                SetNotification("Datos inválidos. Revisa la información del producto.", NotificationType.Error);
                return View(productoVM);
            }

            var producto = new Producto
            {
                Nombre = productoVM.Nombre,
                Descripcion = productoVM.Descripcion,
                PrecioCompra = productoVM.PrecioCompra,
                PrecioVenta = productoVM.PrecioVenta,
                Stock = productoVM.Stock,
                IdCategoria = productoVM.IdCategoria,
                Activo = productoVM.Activo,
                CodigoBarras = productoVM.CodigoBarras
            };

            var resultado = await _productoService.Crear(producto);
            SetNotification(
                resultado ? "Producto creado exitosamente." : "Error al crear el producto.",
                resultado ? NotificationType.Success : NotificationType.Error);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _productoService.obtener(id);
            if (producto == null)
            {
                SetNotification("No se encontró el producto que intentas editar.", NotificationType.Error);
                return RedirectToAction(nameof(Index));
            }

            var productoVM = new ProductoViewModel
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                PrecioCompra = producto.PrecioCompra,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                IdCategoria = producto.IdCategoria,
                NombreCategoria = producto.IdCategoriaNavigation?.Nombre,
                Activo = producto.Activo ?? false,
                CodigoBarras = producto.CodigoBarras
            };

            return View(productoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoViewModel productoVM)
        {
            if (id != productoVM.IdProducto)
            {
                SetNotification("El identificador del producto no coincide.", NotificationType.Error);
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                SetNotification("Datos inválidos. Revisa la información del producto.", NotificationType.Error);
                return View(productoVM);
            }

            var producto = new Producto
            {
                IdProducto = productoVM.IdProducto,
                Nombre = productoVM.Nombre,
                Descripcion = productoVM.Descripcion,
                PrecioCompra = productoVM.PrecioCompra,
                PrecioVenta = productoVM.PrecioVenta,
                Stock = productoVM.Stock,
                IdCategoria = productoVM.IdCategoria,
                Activo = productoVM.Activo,
                CodigoBarras = productoVM.CodigoBarras
            };

            var resultado = await _productoService.Actualizar(producto);
            SetNotification(
                resultado ? "Producto actualizado correctamente." : "Error al actualizar el producto.",
                resultado ? NotificationType.Success : NotificationType.Error);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _productoService.obtener(id);
            if (producto == null)
            {
                SetNotification("No se encontró el producto que intentas eliminar.", NotificationType.Error);
                return RedirectToAction(nameof(Index));
            }

            var productoVM = new ProductoViewModel
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                PrecioCompra = producto.PrecioCompra,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                IdCategoria = producto.IdCategoria,
                NombreCategoria = producto.IdCategoriaNavigation?.Nombre,
                Activo = producto.Activo ?? false,
                CodigoBarras = producto.CodigoBarras
            };

            return View(productoVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultado = await _productoService.Eliminar(id);
            SetNotification(
                resultado ? "Producto eliminado correctamente." : "Error al eliminar el producto.",
                resultado ? NotificationType.Success : NotificationType.Error);

            return RedirectToAction(nameof(Index));
        }
    }
}
