using System;
using System.Collections.Generic;

namespace Juego_Sin_Nombre.Models;

public partial class Game
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public string? Gamestate { get; set; }

    public int? Populationstate { get; set; }

    public int? Armystate { get; set; }

    public int? Economystate { get; set; }

    public int? Magicstate { get; set; }

    public int? Lastcardid { get; set; }

    public int? Day { get; set; }

    public virtual Card? Lastcard { get; set; }

    public virtual Usuario? User { get; set; }
}
