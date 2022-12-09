using System;
using System.Collections.Generic;

namespace CinemaCore.Models;

public partial class ListEvents
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public decimal TicketPrice { get; set; }

    public int FilmId { get; set; }

    public virtual Films Film { get; set; } = null!;

    public virtual ICollection<Places> Places { get; } = new List<Places>();

    public virtual ICollection<StaffCasts> StaffCasts { get; } = new List<StaffCasts>();
}
