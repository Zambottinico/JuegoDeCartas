using System;
using System.Collections.Generic;

namespace Juego_Sin_Nombre.Models;

public partial class Decision
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? Population { get; set; }

    public int? Army { get; set; }

    public int? Economy { get; set; }

    public int? Magic { get; set; }

    public int? UnlockableCharacter { get; set; }

    public virtual ICollection<Card> CardDecision1Navigations { get; set; } = new List<Card>();

    public virtual ICollection<Card> CardDecision2Navigations { get; set; } = new List<Card>();
}
