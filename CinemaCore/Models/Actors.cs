using System;
using System.Collections.Generic;

namespace CinemaCore.Models;

public partial class Actors
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public virtual ICollection<ActorCasts> ActorCasts { get; } = new List<ActorCasts>();
}
