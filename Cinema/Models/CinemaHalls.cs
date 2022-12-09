using System;
using System.Collections.Generic;

namespace CinemaCore.Models;

public partial class CinemaHalls
{
    public int Id { get; set; }

    public int HallNumber { get; set; }

    public int MaxPlaceNumber { get; set; }

    public virtual ICollection<Places> Places { get; } = new List<Places>();
}
