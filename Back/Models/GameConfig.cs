using System.ComponentModel.DataAnnotations;

namespace Juego_Sin_Nombre.Models
{
    public class GameConfig
    {
        public GameConfig() { }

        public GameConfig(int lifeRechargePrice, int daysToEarnDiamond, int minutesToEarnLife)
        {
            LifeRechargePrice = lifeRechargePrice;
            DaysToEarnDiamond = daysToEarnDiamond;
            MinutesToEarnLife = minutesToEarnLife;
        }
        [Key]
        public int Id { get; set; }
        public int LifeRechargePrice { get; set; }
        public int DaysToEarnDiamond { get; set; }
        public int MinutesToEarnLife { get; set; }


    }
}
