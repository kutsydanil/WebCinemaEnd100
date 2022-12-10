using CinemaCore.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebCinema.Models.FilterViewModels
{
    public class FilmFilterViewModel
    {
        public string? SelectedName { get; }

        public int? SelectedDuration { get; }
        public int? SelectedAgeLimit { get; }

        public int SelectedGenre { get; }

        public int SelectedFilmProduction { get; }

        public SelectList FilmProductions { get; }
        public SelectList Genres { get; }

        public FilmFilterViewModel(List<Genres> genres, List<FilmProductions> filmProductions, string? name, int? agelimit, 
            int? duration, int selectedgenre, int selectedfilmproduction)
        {
            genres.Insert(0, new Genres { Name = "Все", Id = 0 });
            Genres = new SelectList(genres, "Id", "Name", selectedgenre);
            SelectedGenre = selectedgenre;

            filmProductions.Insert(0, new FilmProductions { Name = "Все", Id = 0 });
            FilmProductions = new SelectList(filmProductions, "Id", "Name", selectedfilmproduction);

            SelectedName = name;
            SelectedDuration = duration;
            SelectedAgeLimit = agelimit;
        }
    }
}
