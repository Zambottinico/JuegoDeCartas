using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Juego_Sin_Nombre.Models
{
    public class DiamondOfert
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioEnPesos { get; set; }

        public int MontoDeDiamantes { get; set; }

        // Relación con Factura (Una oferta puede estar en varias facturas)
        public List<Invoice> Invoices { get; set; } = new();
        public string Nombre { get; set;}
    }
}
