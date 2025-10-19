using Microsoft.EntityFrameworkCore;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.DAL.ModelosRepositorios
{
    public class UsuarioRepository : IGenericRepository<Usuario>
    {
        private readonly PuntoVentaContext _dbContext;

        public UsuarioRepository(PuntoVentaContext context)
        {
            _dbContext = context;

        }

        public async Task<bool> Crear(Usuario modelo)
        {
            await _dbContext.Usuarios.AddAsync(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(Usuario modelo)
        {
            _dbContext.Usuarios.Update(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            var modelo = await _dbContext.Usuarios.FirstOrDefaultAsync(c => c.IdUsuario == id);
            if (modelo == null)
                return false;

            _dbContext.Usuarios.Remove(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Usuario> obtener(int id)
        {
            return await _dbContext.Usuarios.FindAsync(id);
        }

        public Task<IQueryable<Usuario>> obtenerTodos()
        {
            return Task.FromResult(_dbContext.Usuarios.AsQueryable());
        }
    }
}
