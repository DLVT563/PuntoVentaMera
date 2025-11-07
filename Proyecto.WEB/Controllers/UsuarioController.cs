using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto.BLL.Interfaces;
using Proyecto.MODELS;
using Proyecto.WEB.Models.ViewModels;

namespace Proyecto.WEB.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.obtenerTodos();

            var lista = usuarios.ToList(); 

            var usuariosVM = lista.Select(u => new UsuarioViewModel
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                Usuario1 = u.Usuario1,
                Clave = u.Clave,
                IdRol = u.IdRol,
                NombreRol = ObtenerNombreRol(u.IdRol),
                FechaCreacion = u.FechaCreacion,
                Activo = true
            }).ToList();

            return View(usuariosVM);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _usuarioService.obtener(id);
            if (usuario == null)
            {
                TempData["Error"] = "El usuario no fue encontrado.";
                TempData.Remove("Exito");
                return RedirectToAction(nameof(Index));
            }

            var usuarioVM = new UsuarioViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Usuario1 = usuario.Usuario1,
                Clave = usuario.Clave,
                IdRol = usuario.IdRol,
                NombreRol = ObtenerNombreRol(usuario.IdRol),
                FechaCreacion = usuario.FechaCreacion,
                Activo = true
            };

            return View(usuarioVM);
        }

        [HttpGet]
        public IActionResult Create()
            {
                ViewBag.Roles = ObtenerListaRoles();
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(UsuarioViewModel usuarioVM)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var usuario = new Usuario
                        {
                            Nombre = usuarioVM.Nombre,
                            Usuario1 = usuarioVM.Usuario1,
                            Clave = usuarioVM.Clave,
                            IdRol = usuarioVM.IdRol,
                            FechaCreacion = DateTime.Now
                        };

                        await _usuarioService.Crear(usuario);
                        TempData["Exito"] = "Usuario creado correctamente.";
                        TempData.Remove("Error");
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Error al crear usuario: {ex.Message}";
                        TempData.Remove("Exito");
                    }
                }
                else
                {
                    TempData["Error"] = "Datos inválidos. Verifica los campos.";
                    TempData.Remove("Exito");
                }

                ViewBag.Roles = ObtenerListaRoles();
                return View(usuarioVM);
            }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _usuarioService.obtener(id);
            if (usuario == null)
            {
                TempData["Error"] = "El usuario no fue encontrado.";
                TempData.Remove("Exito");
                return RedirectToAction(nameof(Index));
            }

            var usuarioVM = new UsuarioViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Usuario1 = usuario.Usuario1,
                Clave = usuario.Clave,
                IdRol = usuario.IdRol,
                NombreRol = ObtenerNombreRol(usuario.IdRol),
                FechaCreacion = usuario.FechaCreacion,
                Activo = true
            };

            ViewBag.Roles = ObtenerListaRoles();
            return View(usuarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioViewModel usuarioVM)
        {
            if (id != usuarioVM.IdUsuario)
            {
                TempData["Error"] = "El ID del usuario no coincide.";
                TempData.Remove("Exito");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = new Usuario
                    {
                        IdUsuario = usuarioVM.IdUsuario,
                        Nombre = usuarioVM.Nombre,
                        Usuario1 = usuarioVM.Usuario1,
                        Clave = usuarioVM.Clave,
                        IdRol = usuarioVM.IdRol,
                        FechaCreacion = usuarioVM.FechaCreacion // No se modifica
                    };

                    await _usuarioService.Actualizar(usuario);
                    TempData["Exito"] = "Usuario actualizado correctamente.";
                    TempData.Remove("Error");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al actualizar usuario: {ex.Message}";
                    TempData.Remove("Exito");
                }
            }
            else
            {
                TempData["Error"] = "Datos inválidos. Verifica los campos.";
                TempData.Remove("Exito");
            }

            ViewBag.Roles = ObtenerListaRoles();
            return View(usuarioVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _usuarioService.obtener(id);
            if (usuario == null)
            {
                TempData["Error"] = "El usuario no fue encontrado.";
                TempData.Remove("Exito");
                return RedirectToAction(nameof(Index));
            }

            var usuarioVM = new UsuarioViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Usuario1 = usuario.Usuario1,
                IdRol = usuario.IdRol,
                NombreRol = ObtenerNombreRol(usuario.IdRol),
                FechaCreacion = usuario.FechaCreacion,
                Activo = true
            };

            return View(usuarioVM);
        }

        [HttpGet]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _usuarioService.Eliminar(id);
                TempData["Exito"] = "Usuario eliminado correctamente.";
                TempData.Remove("Error");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar usuario: {ex.Message}";
                TempData.Remove("Exito");
            }

            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> ObtenerListaRoles()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Administrador" },
                new SelectListItem { Value = "2", Text = "Vendedor" },
                new SelectListItem { Value = "3", Text = "Cajero" }
            };
        }

        private static string ObtenerNombreRol(int idRol)
        {
            return idRol switch
            {
                1 => "Administrador",
                2 => "Vendedor",
                3 => "Cajero",
                _ => "Desconocido"
            };
        }
    }
}
