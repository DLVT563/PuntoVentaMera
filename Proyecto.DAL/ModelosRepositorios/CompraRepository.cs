using Microsoft.EntityFrameworkCore;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.DAL.ModelosRepositorios
{
    public class CompraRepository : IGenericRepository<Compra>
    {
        private readonly PuntoVentaContext _dbContext;

        public CompraRepository(PuntoVentaContext context)
        {
            _dbContext = context;
        }

        // Crear una nueva compra
        public async Task<bool> Crear(Compra modelo)
        {
            await _dbContext.Compras.AddAsync(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Actualizar una compra existente
        public async Task<bool> Actualizar(Compra modelo)
        {
            _dbContext.Compras.Update(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Eliminar una compra por Id
        public async Task<bool> Eliminar(int id)
        {
            var modelo = await _dbContext.Compras.FirstOrDefaultAsync(c => c.IdCompra == id);
            if (modelo == null) return false;

            _dbContext.Compras.Remove(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Obtener una compra por Id
        public async Task<Compra?> obtener(int id)
        {
            return await _dbContext.Compras
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.DetalleCompras)
                    .ThenInclude(d => d.IdProductoNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCompra == id);
        }

        // Obtener todas las compras
        public Task<IQueryable<Compra>> obtenerTodos()
        {
            var query = _dbContext.Compras
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.DetalleCompras)
                    .ThenInclude(d => d.IdProductoNavigation)
                .AsNoTracking()
                .AsQueryable();

            return Task.FromResult(query);
        }
    }
}
