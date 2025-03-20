using System.ComponentModel.DataAnnotations;

namespace Juego_Sin_Nombre.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        // Relación con DiamondOffer (Cada invoice tiene UNA oferta)
        public int DiamondOfferId { get; set; }
        public DiamondOfert DiamondOfert { get; set; }
    }
}
