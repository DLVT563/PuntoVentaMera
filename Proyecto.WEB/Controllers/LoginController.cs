using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BLL.Interfaces;
using Proyecto.MODELS;

namespace Proyecto.WEB.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public LoginController(IUsuarioService usuarioservice)
        {
            _usuarioService = usuarioservice;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string clave)
        {
            Usuario? usuarioEncontrado = await _usuarioService.ValidarUsuario(usuario, clave);

            if (usuarioEncontrado == null)
            {
                TempData["Error"] = "Usuario o clave incorrectos";
                TempData.Remove("Exito");
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuarioEncontrado.Nombre),
        new Claim("IdUsuario", usuarioEncontrado.IdUsuario.ToString()),
        new Claim(ClaimTypes.Role, usuarioEncontrado.IdRolNavigation?.Nombre ?? "Usuario")
    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties);

            return RedirectToAction("Crear", "Ventas"); 
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
