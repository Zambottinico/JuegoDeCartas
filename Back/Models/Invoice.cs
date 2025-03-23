using Juego_Sin_Nombre.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Juego_Sin_Nombre.Models
{
    public class Invoice
    {
        public Invoice(InvoiceStatus status, int diamondOfferId, DiamondOfert diamondOfert)
        {
            
            Status = status;
            DiamondOfferId = diamondOfferId;
            DiamondOfert = diamondOfert;
            CreatedAt = DateTime.Now;
        }
        private Invoice() { }


        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        
        public InvoiceStatus Status { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Relación con DiamondOffer (Cada invoice tiene UNA oferta)
        public int DiamondOfferId { get; set; }
        public DiamondOfert DiamondOfert { get; set; }
    }
}
