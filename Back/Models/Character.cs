using System;
using System.Collections.Generic;

namespace Juego_Sin_Nombre.Models;

public partial class Character
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual ICollection<Usuario> Players { get; set; } = new List<Usuario>();
}
