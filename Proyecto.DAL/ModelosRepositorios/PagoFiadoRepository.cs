using Microsoft.EntityFrameworkCore;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.DAL.ModelosRepositorios
{
    public class PagoFiadoRepository : IGenericRepository<PagoFiado>
    {
        private readonly PuntoVentaContext _dbContext;

        public PagoFiadoRepository(PuntoVentaContext context)
        {
            _dbContext = context;
        }

        // Crear un nuevo pago fiado
        public async Task<bool> Crear(PagoFiado modelo)
        {
            await _dbContext.PagoFiados.AddAsync(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Actualizar un pago fiado existente
        public async Task<bool> Actualizar(PagoFiado modelo)
        {
            _dbContext.PagoFiados.Update(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Eliminar un pago fiado por Id
        public async Task<bool> Eliminar(int id)
        {
            var modelo = await _dbContext.PagoFiados.FirstOrDefaultAsync(p => p.IdPago == id);
            if (modelo == null) return false;

            _dbContext.PagoFiados.Remove(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Obtener un pago fiado por Id
        public async Task<PagoFiado?> obtener(int id)
        {
            return await _dbContext.PagoFiados
                .Include(p => p.IdVentaNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdPago == id);
        }

        // Obtener todos los pagos fiados
        public Task<IQueryable<PagoFiado>> obtenerTodos()
        {
            var query = _dbContext.PagoFiados
                .Include(p => p.IdVentaNavigation)
                .AsNoTracking()
                .AsQueryable();

            return Task.FromResult(query);
        }
    }
}
