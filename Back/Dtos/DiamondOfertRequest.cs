namespace Juego_Sin_Nombre.Dtos
{
    public class DiamondOfertRequest
    {
        public int UserId { get; set; }
        public string Clave { get; set; }
        public decimal PrecioEnPesos { get; set; }
        public int MontoDeDiamantes { get; set; }
        public string Nombre { get; set; }
    }
}
