namespace Juego_Sin_Nombre.Dtos
{
    public class UnlockCharacterRequest
    {
        public int UserId { get; set; }
        public string Clave { get; set; } = string.Empty;
        public int IdCardOfert { get; set; }
    }
}
