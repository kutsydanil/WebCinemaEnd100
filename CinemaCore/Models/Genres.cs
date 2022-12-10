using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CinemaCore.Models;

public partial class Genres
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Не указан жанр")]
    [Display(Name = "Жанр")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Films> Films { get; } = new List<Films>();
}
