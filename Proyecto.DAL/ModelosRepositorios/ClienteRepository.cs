using Microsoft.EntityFrameworkCore;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.DAL.ModelosRepositorios
{
    public class ClienteRepository : IGenericRepository<Cliente>
    {
        private readonly PuntoVentaContext _dbContext;

        public ClienteRepository(PuntoVentaContext context)
        {
            _dbContext = context;
        }
        public async Task<bool> Actualizar(Cliente modelo)
        {
            _dbContext.Clientes.Update(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Crear(Cliente modelo)
        {
            await _dbContext.Clientes.AddAsync(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            var modelo = await _dbContext.Clientes.FirstOrDefaultAsync(p => p.IdCliente == id);
            if (modelo == null)
                return false;

            _dbContext.Clientes.Remove(modelo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Cliente> obtener(int id)
        {
            return await _dbContext.Clientes.FindAsync(id);
        }

        public Task<IQueryable<Cliente>> obtenerTodos()
        {
            return Task.FromResult(_dbContext.Clientes.AsNoTracking().AsQueryable());

        }
    }
}
