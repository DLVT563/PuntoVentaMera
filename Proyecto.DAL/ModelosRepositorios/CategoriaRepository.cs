using Microsoft.EntityFrameworkCore;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.DAL.ModelosRepositorios
{
    public class CategoriaRepository : IGenericRepository<Categorium>
    {
        private readonly PuntoVentaContext _dbContext;

        public CategoriaRepository(PuntoVentaContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> Crear(Categorium modelo)
        {
            await _dbContext.Categoria.AddAsync(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(Categorium modelo)
        {
            _dbContext.Categoria.Update(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            var modelo = await _dbContext.Productos.FirstOrDefaultAsync(p => p.IdCategoria == id);
            if (modelo == null)
                return false;

            _dbContext.Productos.Remove(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Categorium> obtener(int id)
        {
            return await _dbContext.Categoria
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.IdCategoria == id);
        }

        public Task<IQueryable<Categorium>> obtenerTodos()
        {
            IQueryable<Categorium> query = _dbContext.Categoria
                .Include(c => c.Productos);
            return Task.FromResult(query);
        }
    }
}
