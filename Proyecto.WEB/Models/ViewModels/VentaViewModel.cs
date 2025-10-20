using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.WEB.Models.ViewModels
{
    public class VentaViewModel
    {
        [Key]
        public int IdVenta { get; set; }

        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Display(Name = "Total")]
        [Range(0, double.MaxValue)]
        public decimal Total { get; set; }

        [Display(Name = "Saldo Pendiente")]
        public decimal? SaldoPendiente { get; set; }

        [Display(Name = "¿Es fiado?")]
        public bool EsFiado { get; set; } = false;

        [Display(Name = "Pagado")]
        public bool Pagado { get; set; } = true;

        [Display(Name = "Fecha de Pago Estimada")]
        public DateOnly? FechaPagoEstimada { get; set; }

        [Display(Name = "Número de Venta")]
        public string? NumeroVenta { get; set; }

        [Display(Name = "Tipo de Comprobante")]
        public string? TipoComprobante { get; set; }

        [Display(Name = "Serie")]
        public string? Serie { get; set; }

        [Display(Name = "Número de Documento")]
        public string? NumeroDocumento { get; set; }

        [Display(Name = "Método de Pago")]
        public string? MetodoPago { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [Display(Name = "Anulado")]
        public bool Anulado { get; set; } = false;

        [Required(ErrorMessage = "Debe agregar al menos un producto a la venta")]
        public List<DetalleVentaViewModel> DetalleVenta { get; set; } = new List<DetalleVentaViewModel>();

        // Productos disponibles para selección al crear venta
        public List<ProductoViewModel> ProductosDisponibles { get; set; } = new List<ProductoViewModel>();

        // Historial de stock asociado a la venta
        public List<MovimientoStockViewModel>? HistorialStock { get; set; }
    }

    public class DetalleVentaViewModel
    {
        [Key]
        public int IdDetalle { get; set; }

        [Display(Name = "Producto")]
        public int IdProducto { get; set; }

        [Display(Name = "Nombre del producto")]
        public string NombreProducto { get; set; } = string.Empty;

        [Display(Name = "Cantidad")]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Display(Name = "Precio unitario")]
        [Range(0, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Subtotal")]
        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }
    }

    public class MovimientoStockViewModel
    {
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Referencia { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public int? IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
    }
}
