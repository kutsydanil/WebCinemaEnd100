using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CinemaCore.Models;

public partial class Actors
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Не указано имя актера")]
    [Display(Name = "Имя")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Не указана фамилия актера")]
    [Display(Name = "Фамилия")]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "Не указано отчество сотрудника")]
    [Display(Name = "Отчество")]
    public string MiddleName { get; set; } = null!;

    public virtual ICollection<ActorCasts> ActorCasts { get; } = new List<ActorCasts>();
}
