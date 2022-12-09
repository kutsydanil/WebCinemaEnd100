using System;
using System.Collections.Generic;

namespace CinemaCore.Models;

public partial class Films
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int GenreId { get; set; }

    public int Duration { get; set; }

    public int FilmProductionId { get; set; }

    public int CountryProductionId { get; set; }

    public int AgeLimit { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<ActorCasts> ActorCasts { get; } = new List<ActorCasts>();

    public virtual CountryProductions CountryProduction { get; set; } = null!;

    public virtual FilmProductions FilmProduction { get; set; } = null!;

    public virtual Genres Genre { get; set; } = null!;

    public virtual ICollection<ListEvents> ListEvents { get; } = new List<ListEvents>();
}
