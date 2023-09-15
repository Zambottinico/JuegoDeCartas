using Juego_Sin_Nombre.Models;

namespace Juego_Sin_Nombre.Dtos
{
    public class PostDecisionDto
    {
        

        public string? Description { get; set; }

        public int? Population { get; set; }

        public int? Army { get; set; }

        public int? Economy { get; set; }

        public int? Magic { get; set; }
        public int? UnlockableCharacter { get; set; }

        internal void Convert(Decision? decision)
        {
            Description = decision.Description;
            Population = decision.Population;
            Army = decision.Army;
            Economy = decision.Economy;
            Magic = decision.Magic;
            UnlockableCharacter=decision.unlockable_character;
        }
    }
}
