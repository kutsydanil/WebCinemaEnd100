using System;
using System.Collections.Generic;

namespace CinemaCore.Models;

public partial class CountryProductions
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Films> Films { get; } = new List<Films>();
}
