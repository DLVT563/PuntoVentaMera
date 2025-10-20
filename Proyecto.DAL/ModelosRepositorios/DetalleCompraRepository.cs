
using Microsoft.EntityFrameworkCore;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.DAL.ModelosRepositorios
{
    public class DetalleCompraRepository : IGenericRepository<DetalleCompra>
    {
        private readonly PuntoVentaContext _dbContext;

        public DetalleCompraRepository(PuntoVentaContext context)
        {
            _dbContext = context;
        }

        // Crear detalle de compra
        public async Task<bool> Crear(DetalleCompra modelo)
        {
            await _dbContext.DetalleCompras.AddAsync(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Actualizar detalle de compra
        public async Task<bool> Actualizar(DetalleCompra modelo)
        {
            _dbContext.DetalleCompras.Update(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Eliminar detalle de compra por id
        public async Task<bool> Eliminar(int id)
        {
            var modelo = await _dbContext.DetalleCompras.FirstOrDefaultAsync(d => d.IdDetalle == id);
            if (modelo == null) return false;

            _dbContext.DetalleCompras.Remove(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Obtener detalle de compra por id
        public async Task<DetalleCompra?> obtener(int id)
        {
            return await _dbContext.DetalleCompras
                .Include(d => d.IdCompraNavigation)
                .Include(d => d.IdProductoNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDetalle == id);
        }

        // Obtener todos los detalles de compra
        public Task<IQueryable<DetalleCompra>> obtenerTodos()
        {
            var query = _dbContext.DetalleCompras
                .Include(d => d.IdCompraNavigation)
                .Include(d => d.IdProductoNavigation)
                .AsNoTracking()
                .AsQueryable();

            return Task.FromResult(query);
        }
    }
}

