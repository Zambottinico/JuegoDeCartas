using Juego_Sin_Nombre.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Juego_Sin_Nombre.Models
{
    public class Invoice
    {
        public Invoice(string preferenceId, InvoiceStatus status, int diamondOfferId, DiamondOfert diamondOfert)
        {
            PreferenceId = preferenceId;
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
        public string PreferenceId { get; set; }
        public InvoiceStatus Status { get; set; }

        // Relación con DiamondOffer (Cada invoice tiene UNA oferta)
        public int DiamondOfferId { get; set; }
        public DiamondOfert DiamondOfert { get; set; }
    }
}
