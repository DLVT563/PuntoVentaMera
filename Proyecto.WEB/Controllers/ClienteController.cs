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
    [Authorize(Roles = "Administrador, Vendedor")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.obtenerTodos();
            var lista = clientes.ToList();

            var clientesVM = lista.Select(c => new ClienteViewModel
            {
                IdCliente = c.IdCliente,
                Nombre = c.Nombre,
                Telefono = c.Telefono,
                Email = c.Email,
                Direccion = c.Direccion,
                Activo = true
            }).ToList();

            return View(clientesVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteService.obtener(id);
            if (cliente == null)
            {
                TempData["Error"] = "El cliente no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var clienteVM = new ClienteViewModel
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Direccion = cliente.Direccion,
                Activo = true
            };

            return View(clienteVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel clienteVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = new Cliente
                    {
                        Nombre = clienteVM.Nombre,
                        Telefono = clienteVM.Telefono,
                        Email = clienteVM.Email,
                        Direccion = clienteVM.Direccion
                    };

                    await _clienteService.Crear(cliente);
                    TempData["Exito"] = "Cliente creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al crear cliente: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Datos inválidos. Verifica los campos.";
            }

            return View(clienteVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.obtener(id);
            if (cliente == null)
            {
                TempData["Error"] = "El cliente no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var clienteVM = new ClienteViewModel
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Direccion = cliente.Direccion,
                Activo = true
            };

            return View(clienteVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel clienteVM)
        {
            if (id != clienteVM.IdCliente)
            {
                TempData["Error"] = "El ID del cliente no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = new Cliente
                    {
                        IdCliente = clienteVM.IdCliente,
                        Nombre = clienteVM.Nombre,
                        Telefono = clienteVM.Telefono,
                        Email = clienteVM.Email,
                        Direccion = clienteVM.Direccion
                    };

                    await _clienteService.Actualizar(cliente);
                    TempData["Exito"] = "Cliente actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al actualizar cliente: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Datos inválidos. Verifica los campos.";
            }

            return View(clienteVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteService.obtener(id);
            if (cliente == null)
            {
                TempData["Error"] = "El cliente no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var clienteVM = new ClienteViewModel
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Direccion = cliente.Direccion,
                Activo = true
            };

            return View(clienteVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _clienteService.Eliminar(id);
                TempData["Exito"] = "Cliente eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar cliente: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
