using System;
using System.Collections.Generic;

namespace Juego_Sin_Nombre.Models;

public partial class Card
{
    public int Id { get; set; }

    public int? Typeid { get; set; }

    public string? Description { get; set; }

    public int? Decision1 { get; set; }

    public int? Decision2 { get; set; }

    public bool? IsPlayable { get; set; }

    public int? CharacterId { get; set; }

    public virtual Character? Character { get; set; }

    public virtual Decision? Decision1Navigation { get; set; }

    public virtual Decision? Decision2Navigation { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual Type? Type { get; set; }
}
