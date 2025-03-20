namespace Juego_Sin_Nombre.Dtos
{
    public class UpdateCardOfertRequest
    {
        public int Userid { get; set; }
        public int Id { get; set; }
        public string clave { get; set; }
        public int CharacterId { get; set; }
        public int GoldPrice { get; set; }
        public int DiamondPrice { get; set; }
    }
}
