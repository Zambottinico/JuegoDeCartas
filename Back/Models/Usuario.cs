﻿ using System;
using System.Collections.Generic;

namespace Juego_Sin_Nombre.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Email { get; set; }
    public string? Username { get; set; }

    public string Password { get; set; }

    public int? Maxdays { get; set; }
    public int? MaxLives { get; set; }
    public int? Lives { get; set; }
    public DateTime? LastLifeRecharge { get; set; }  // Última vez que se regeneró una vida
    public int? Gold { get; set; }
    public int? Diamonds { get; set; }

    public string? Clave { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<Cupon> Cupons { get; set; } = new List<Cupon>();
}
