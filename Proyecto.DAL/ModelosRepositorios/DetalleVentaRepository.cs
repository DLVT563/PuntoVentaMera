using Microsoft.EntityFrameworkCore;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.DAL.ModelosRepositorios
{
    public class DetalleVentaRepository : IGenericRepository<DetalleVentum>
    {
        private readonly PuntoVentaContext _dbContext;

        public DetalleVentaRepository(PuntoVentaContext context)
        {
            _dbContext = context;
        }

        // Crear detalle de venta
        public async Task<bool> Crear(DetalleVentum modelo)
        {
            await _dbContext.DetalleVenta.AddAsync(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Actualizar detalle de venta
        public async Task<bool> Actualizar(DetalleVentum modelo)
        {
            _dbContext.DetalleVenta.Update(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Eliminar detalle de venta por id
        public async Task<bool> Eliminar(int id)
        {
            var modelo = await _dbContext.DetalleVenta.FirstOrDefaultAsync(d => d.IdDetalle == id);
            if (modelo == null) return false;

            _dbContext.DetalleVenta.Remove(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Obtener detalle de venta por id
        public async Task<DetalleVentum?> obtener(int id)
        {
            return await _dbContext.DetalleVenta
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdVentaNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDetalle == id);
        }

        // Obtener todos los detalles de venta
        public Task<IQueryable<DetalleVentum>> obtenerTodos()
        {
            var query = _dbContext.DetalleVenta
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdVentaNavigation)
                .AsNoTracking()
                .AsQueryable();

            return Task.FromResult(query);
        }
    }
}
