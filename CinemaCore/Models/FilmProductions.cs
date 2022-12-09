using System;
using System.Collections.Generic;

namespace CinemaCore.Models;

public partial class FilmProductions
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<Films> Films { get; } = new List<Films>();
}
