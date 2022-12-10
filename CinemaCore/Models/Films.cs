﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaCore.Models;

public partial class Films
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Не указано название фильма")]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Не указано названи жанр")]
    [Display(Name = "Жанр")]
    public int GenreId { get; set; }

    [Required(ErrorMessage = "Не указана длительность фильма")]
    [Display(Name = "Длительность")]
    [Range(5, 500, ErrorMessage = "Недопустимая длительность фильма")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "Не указана компания-производитель")]
    [Display(Name = "Компания-производитель")]
    public int FilmProductionId { get; set; }

    [Required(ErrorMessage = "Не указана страна-производитель")]
    [Display(Name = "Страна-производитель")]
    public int CountryProductionId { get; set; }

    [Required(ErrorMessage = "Не указан возраст")]
    [Display(Name = "Возраст")]
    [Range(1, 110, ErrorMessage = "Недопустимый возраст")]
    public int AgeLimit { get; set; }

    [Required(ErrorMessage = "Не указано описание")]
    [Display(Name = "Описание")]
    public string Description { get; set; } = null!;

    public virtual ICollection<ActorCasts> ActorCasts { get; } = new List<ActorCasts>();

    public virtual CountryProductions CountryProduction { get; set; }

    public virtual FilmProductions FilmProduction { get; set; }

    public virtual Genres Genre { get; set; }

    public virtual ICollection<ListEvents> ListEvents { get; } = new List<ListEvents>();
}
