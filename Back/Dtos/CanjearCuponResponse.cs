namespace Juego_Sin_Nombre.Dtos
{
    public class CanjearCuponResponse
    {
        public bool Ok { get; set; }
        public string message { get; set; } 
        public int NumeroDiamantes { get; set; }
        public int NumeroOro { get; set; }
        public string Codigo { get; set; }
    }
}
