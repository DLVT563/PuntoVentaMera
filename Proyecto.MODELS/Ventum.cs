using System;
using System.Collections.Generic;

namespace Proyecto.MODELS
{
    public partial class Ventum
    {
        public int IdVenta { get; set; }

        public int? IdCliente { get; set; }

        public int IdUsuario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public decimal Total { get; set; }

        public bool EsFiado { get; set; } = false;

        public decimal? SaldoPendiente { get; set; }

        public bool Pagado { get; set; } = true;

        public DateOnly? FechaPagoEstimada { get; set; }

        public string NumeroVenta { get; set; } = string.Empty; 

        public string? TipoComprobante { get; set; } 

        public string? Serie { get; set; }            

        public string? NumeroDocumento { get; set; }  

        public string? MetodoPago { get; set; }    

        public string? Observaciones { get; set; }

        public bool Anulado { get; set; } = false;

        public DateTime? FechaAnulacion { get; set; }

        // === RELACIONES ===
        public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

        public virtual Cliente? IdClienteNavigation { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

        public virtual ICollection<PagoFiado> PagoFiados { get; set; } = new List<PagoFiado>();
    }
}
