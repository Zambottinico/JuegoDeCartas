namespace Juego_Sin_Nombre.Dtos
{
    public class LoginResponse
    {
        public string Clave { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public int id { get; set; }
        public string Rol { get; set; }
        public bool Ok { get; set; }
    }
}
