using Microsoft.EntityFrameworkCore;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.DAL.ModelosRepositorios
{
    public class VentasRepository : IGenericRepository<Ventum>
    {
        private readonly PuntoVentaContext _dbContext;
    public VentasRepository(PuntoVentaContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> Crear(Ventum modelo)
        {
            await _dbContext.Venta.AddAsync(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(Ventum modelo)
        {
            _dbContext.Venta.Update(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            var modelo = await _dbContext.Venta.FirstOrDefaultAsync(v => v.IdVenta == id);
            if (modelo == null)
                return false;

            _dbContext.Venta.Remove(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Ventum> obtener(int id)
        {
            return await _dbContext.Venta
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.IdVenta == id);
        }

        public Task<IQueryable<Ventum>> obtenerTodos()
        {
            return Task.FromResult(_dbContext.Venta
                .AsNoTracking()
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation)
                .AsQueryable());
        }
    }
}
