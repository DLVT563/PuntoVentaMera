using Microsoft.AspNetCore.Mvc;
using Proyecto.BLL.Interfaces;
using Proyecto.MODELS;
using Proyecto.WEB.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.WEB.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.obtenerTodos();

            var lista = categorias.ToList();

            var categoriasVM = lista.Select(c => new CategoriaViewModel
            {
                IdCategoria = c.IdCategoria,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Activo = true
            }).ToList();

            return View(categoriasVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var categoria = await _categoriaService.obtener(id);
            if (categoria == null)
            {
                TempData["Error"] = "La categoría no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var categoriaVM = new CategoriaViewModel
            {
                IdCategoria = categoria.IdCategoria,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = true
            };

            return View(categoriaVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaViewModel categoriaVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoria = new Categorium
                    {
                        Nombre = categoriaVM.Nombre,
                        Descripcion = categoriaVM.Descripcion
                    };

                    await _categoriaService.Crear(categoria);
                    TempData["Exito"] = "Categoría creada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al crear categoría: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Datos inválidos. Verifica los campos.";
            }

            return View(categoriaVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _categoriaService.obtener(id);
            if (categoria == null)
            {
                TempData["Error"] = "La categoría no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var categoriaVM = new CategoriaViewModel
            {
                IdCategoria = categoria.IdCategoria,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = true
            };

            return View(categoriaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaViewModel categoriaVM)
        {
            if (id != categoriaVM.IdCategoria)
            {
                TempData["Error"] = "El ID de la categoría no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var categoria = new Categorium
                    {
                        IdCategoria = categoriaVM.IdCategoria,
                        Nombre = categoriaVM.Nombre,
                        Descripcion = categoriaVM.Descripcion
                    };

                    await _categoriaService.Actualizar(categoria);
                    TempData["Exito"] = "Categoría actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al actualizar categoría: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Datos inválidos. Verifica los campos.";
            }

            return View(categoriaVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _categoriaService.obtener(id);
            if (categoria == null)
            {
                TempData["Error"] = "La categoría no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var categoriaVM = new CategoriaViewModel
            {
                IdCategoria = categoria.IdCategoria,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = true
            };

            return View(categoriaVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _categoriaService.Eliminar(id);
                TempData["Exito"] = "Categoría eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar categoría: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
