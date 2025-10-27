using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto.BLL.Interfaces;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;

namespace Proyecto.BLL.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        public UsuarioService(IGenericRepository<Usuario> usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<bool> Crear(Usuario modelo)
        {
            try
            {
                return await _usuarioRepositorio.Crear(modelo);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Actualizar(Usuario modelo)
        {
            try
            {
                return await _usuarioRepositorio.Actualizar(modelo);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                return await _usuarioRepositorio.Eliminar(id);
            }
            catch
            {
                return false;   
            }
        }

        public async Task<Usuario> obtener(int id)
        {
            return await _usuarioRepositorio.obtener(id);
        }

        public async Task<IQueryable<Usuario>> obtenerTodos()
        {
            return await _usuarioRepositorio.obtenerTodos();
        }

        public async Task<Usuario?> ValidarUsuario(string nombreUsuario, string clave)
        {
            var usuarios = await _usuarioRepositorio.obtenerTodos();
            return usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefault(u =>
                    u.Nombre == nombreUsuario &&
                    u.Clave == clave
                );
        }
    }
}
