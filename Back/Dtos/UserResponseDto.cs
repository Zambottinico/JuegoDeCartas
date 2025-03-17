namespace Juego_Sin_Nombre.Dtos
{
    public class UserResponseDto
    {
        

        public string Username { get; set; }
        public int MaxDays { get; set; }
        public int Gold { get; set; }
        public int Diamonds { get; set; }
        public int? MaxLives { get; set; }
        public int? Lives { get; set; }
        public DateTime? LastLifeRecharge { get; set; }
    }
}
