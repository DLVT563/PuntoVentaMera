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
    [Authorize(Roles = "Administrador")]
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var proveedores = await _proveedorService.obtenerTodos();

            var lista = proveedores.ToList();

            var proveedoresVM = lista.Select(p => new ProveedorViewModel
            {
                IdProveedor = p.IdProveedor,
                Nombre = p.Nombre,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion,
                Activo = true
            }).ToList();

            return View(proveedoresVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var proveedor = await _proveedorService.obtener(id);
            if (proveedor == null)
            {
                TempData["Error"] = "El proveedor no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var proveedorVM = new ProveedorViewModel
            {
                IdProveedor = proveedor.IdProveedor,
                Nombre = proveedor.Nombre,
                Telefono = proveedor.Telefono,
                Email = proveedor.Email,
                Direccion = proveedor.Direccion,
                Activo = true
            };

            return View(proveedorVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProveedorViewModel proveedorVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var proveedor = new Proveedor
                    {
                        Nombre = proveedorVM.Nombre,
                        Telefono = proveedorVM.Telefono,
                        Email = proveedorVM.Email,
                        Direccion = proveedorVM.Direccion
                    };

                    await _proveedorService.Crear(proveedor);
                    TempData["Exito"] = "Proveedor creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al crear proveedor: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Datos inválidos. Verifica los campos.";
            }

            return View(proveedorVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var proveedor = await _proveedorService.obtener(id);
            if (proveedor == null)
            {
                TempData["Error"] = "El proveedor no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var proveedorVM = new ProveedorViewModel
            {
                IdProveedor = proveedor.IdProveedor,
                Nombre = proveedor.Nombre,
                Telefono = proveedor.Telefono,
                Email = proveedor.Email,
                Direccion = proveedor.Direccion,
                Activo = true
            };

            return View(proveedorVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProveedorViewModel proveedorVM)
        {
            if (id != proveedorVM.IdProveedor)
            {
                TempData["Error"] = "El ID del proveedor no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var proveedor = new Proveedor
                    {
                        IdProveedor = proveedorVM.IdProveedor,
                        Nombre = proveedorVM.Nombre,
                        Telefono = proveedorVM.Telefono,
                        Email = proveedorVM.Email,
                        Direccion = proveedorVM.Direccion
                    };

                    await _proveedorService.Actualizar(proveedor);
                    TempData["Exito"] = "Proveedor actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al actualizar proveedor: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Datos inválidos. Verifica los campos.";
            }

            return View(proveedorVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _proveedorService.obtener(id);
            if (proveedor == null)
            {
                TempData["Error"] = "El proveedor no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var proveedorVM = new ProveedorViewModel
            {
                IdProveedor = proveedor.IdProveedor,
                Nombre = proveedor.Nombre,
                Telefono = proveedor.Telefono,
                Email = proveedor.Email,
                Direccion = proveedor.Direccion,
                Activo = true
            };

            return View(proveedorVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _proveedorService.Eliminar(id);
                TempData["Exito"] = "Proveedor eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar proveedor: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
