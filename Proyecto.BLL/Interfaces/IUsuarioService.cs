using Proyecto.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> Crear(Usuario modelo);
        Task<bool> Actualizar(Usuario modelo);
        Task<bool> Eliminar(int id);
        Task<Usuario> obtener(int id);
        Task<IQueryable<Usuario>> obtenerTodos();
        Task<Usuario?> ValidarUsuario(string nombreUsuario, string clave);
    }
}
