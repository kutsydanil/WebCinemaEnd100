using System;
using System.Collections.Generic;

namespace CinemaCore.Models;

public partial class Staffs
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string Post { get; set; } = null!;

    public int WorkExperience { get; set; }

    public virtual ICollection<StaffCasts> StaffCasts { get; } = new List<StaffCasts>();
}
