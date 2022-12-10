using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CinemaCore.Models;

public partial class Staffs
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Не указано имя сотрудника")]
    [Display(Name = "Имя")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Не указана фамилия сотрудника")]
    [Display(Name = "Фамилия")]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "Не указано отчество сотрудника")]
    [Display(Name = "Отчество")]
    public string MiddleName { get; set; } = null!;

    [Required(ErrorMessage = "Не указана должность сотрудника")]
    [Display(Name = "Должность")]
    public string Post { get; set; } = null!;

    [Required(ErrorMessage = "Не указан опыт работы")]
    [Display(Name = "Опыт работы")]
    [Range(0, 1200, ErrorMessage = "Недопустимое значение опыта работы")]
    public int WorkExperience { get; set; }

    public virtual ICollection<StaffCasts> StaffCasts { get; } = new List<StaffCasts>();
}
