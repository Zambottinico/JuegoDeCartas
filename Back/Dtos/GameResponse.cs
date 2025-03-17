namespace Juego_Sin_Nombre.Dtos
{
    public class GameResponse
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Gamestate { get; set; }

        public int Populationstate { get; set; }

        public int Armystate { get; set; }

        public int Economystate { get; set; }

        public int Magicstate { get; set; }

        public CardResponse LastCard { get; set; }

        public int Day { get; set; }
        public int? Gold { get; set; }
        public int? Diamonds { get; set; }
        public int? Lives { get; set; }
        public bool characterUnlocked { get; set; } = false;
        public string unlockableCharacterName { get;set; }
    }
}
