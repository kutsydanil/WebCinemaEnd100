using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CinemaCore.Models;

public partial class FilmProductions
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Не указано название компании-производителя")]
    [Display(Name = "Компания-производитель")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Films> Films { get; } = new List<Films>();
}
