namespace Juego_Sin_Nombre.Models
{
    public class Cupon
    {
        public int Id { get; set; } 
        public int NumeroDiamantes { get; set; } 
        public int NumeroOro { get; set; } 
        public string Codigo { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Usuario> Players { get; set; } = new List<Usuario>();
    }
}
