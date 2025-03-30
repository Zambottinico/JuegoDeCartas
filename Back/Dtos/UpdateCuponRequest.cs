namespace Juego_Sin_Nombre.Dtos
{
    public class UpdateCuponRequest
    {
        public int Id { get; set; }
        public int NumeroDiamantes { get; set; }
        public int NumeroOro { get; set; }
        public string Codigo { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
