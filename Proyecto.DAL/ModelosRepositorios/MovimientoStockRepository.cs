using Microsoft.EntityFrameworkCore;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.DAL.ModelosRepositorios
{
    public class MovimientoStockRepository : IGenericRepository<MovimientoStock>
    {
        private readonly PuntoVentaContext _dbContext;

        public MovimientoStockRepository(PuntoVentaContext context)
        {
            _dbContext = context;
        }
        public async Task<bool> Crear(MovimientoStock modelo)
        {
            await _dbContext.MovimientoStocks.AddAsync(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(MovimientoStock modelo)
        {
            _dbContext.MovimientoStocks.Update(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            var modelo = await _dbContext.MovimientoStocks.FirstOrDefaultAsync(m => m.IdMovimiento == id);
            if (modelo == null) return false;

            _dbContext.MovimientoStocks.Remove(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<MovimientoStock?> obtener(int id)
        {
            return await _dbContext.MovimientoStocks
                .Include(m => m.IdProductoNavigation)
                .Include(m => m.IdUsuarioNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdMovimiento == id);
        }

        public Task<IQueryable<MovimientoStock>> obtenerTodos()
        {
            var query = _dbContext.MovimientoStocks
                .Include(m => m.IdProductoNavigation)
                .Include(m => m.IdUsuarioNavigation)
                .AsNoTracking()
                .AsQueryable();

            return Task.FromResult(query);
        }
    }
}
