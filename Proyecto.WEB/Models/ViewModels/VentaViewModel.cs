using Proyecto.MODELS;
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

        [Required]
        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Display(Name = "Total")]
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0")]
        public decimal Total { get; set; }

        [Display(Name = "¿Es fiado?")]
        public bool EsFiado { get; set; } = false;

        [Display(Name = "Saldo pendiente")]
        [Range(0, double.MaxValue, ErrorMessage = "El saldo pendiente debe ser mayor o igual a 0")]
        public decimal? SaldoPendiente { get; set; }

        [Display(Name = "Pagado")]
        public bool Pagado { get; set; } = true;

        [Display(Name = "Fecha estimada de pago")]
        public DateOnly? FechaPagoEstimada { get; set; }

        [Display(Name = "Número de venta")]
        [StringLength(50, ErrorMessage = "El número de venta no puede exceder 50 caracteres")]
        public string NumeroVenta { get; set; } = string.Empty;

        [Display(Name = "Tipo de comprobante")]
        [StringLength(50, ErrorMessage = "El tipo de comprobante no puede exceder 50 caracteres")]
        public string? TipoComprobante { get; set; }

        [Display(Name = "Serie")]
        [StringLength(50, ErrorMessage = "La serie no puede exceder 50 caracteres")]
        public string? Serie { get; set; }

        [Display(Name = "Número de documento")]
        [StringLength(50, ErrorMessage = "El número de documento no puede exceder 50 caracteres")]
        public string? NumeroDocumento { get; set; }

        [Display(Name = "Método de pago")]
        [StringLength(50, ErrorMessage = "El método de pago no puede exceder 50 caracteres")]
        public string? MetodoPago { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }

        [Display(Name = "Anulado")]
        public bool Anulado { get; set; } = false;

        // Lista de productos agregados a la venta
        [Required(ErrorMessage = "Debe agregar al menos un producto a la venta")]
        public List<DetalleVentaViewModel> DetalleVenta { get; set; } = new List<DetalleVentaViewModel>();

        // Productos disponibles para selección en la venta
        public List<ProductoViewModel> ProductosDisponibles { get; set; } = new List<ProductoViewModel>();
    }

    public class DetalleVentaViewModel
    {
        [Key]
        public int IdDetalle { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un producto")]
        [Display(Name = "Producto")]
        public int IdProducto { get; set; }

        [Display(Name = "Nombre del producto")]
        public string NombreProducto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe indicar la cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio unitario")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Subtotal")]
        [Range(0, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor o igual a 0")]
        public decimal Subtotal { get; set; }
    }
}
