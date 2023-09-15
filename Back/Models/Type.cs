using System;
using System.Collections.Generic;

namespace Juego_Sin_Nombre.Models;

public partial class Type
{
    public int Id { get; set; }

    public string? Type1 { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
