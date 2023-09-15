namespace Juego_Sin_Nombre.Dtos
{
    public class CardResponse
    {
        public int Typeid { get; set; }

        public string Description { get; set; }
        public string Character { get; set; }
        public PostDecisionDto Decision1 { get; set; }

        public PostDecisionDto Decision2 { get; set; }

        
    }
}
