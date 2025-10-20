using Proyecto.MODELS;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.BLL.Interfaces
{
    public interface IVentasService
    {
        Task<Ventum> GuardarVenta(Ventum venta);                         
        Task<Ventum?> Obtener(int id);                                   
        Task<Ventum?> ObtenerConDetalle(int id);                         
        Task<IQueryable<Ventum>> ObtenerTodos();                         
        Task<decimal> CalcularTotalVenta(Ventum venta);                 
        Task<bool> ActualizarStockDespuesDeVenta(Ventum venta);          
        Task<bool> AnularVenta(int idVenta);                             
        Task<bool> EliminarProductoDeVenta(int idVenta, int idDetalle);
        Task<List<MovimientoStock>> ObtenerHistorialStockPorVenta(int idVenta);
    }
}
