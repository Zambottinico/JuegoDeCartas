using System;
using System.Collections.Generic;

namespace Juego_Sin_Nombre.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? Maxdays { get; set; }

    public string? Clave { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}
