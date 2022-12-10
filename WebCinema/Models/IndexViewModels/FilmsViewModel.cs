using CinemaCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebCinema.Models;
using WebCinema.Models.PageViewModels;
using WebCinema.Models.FilterViewModels;
using WebCinema.Models.SortViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebCinema.Models.IndexViewModels
{
    public class FilmsViewModel
    {
        public IEnumerable<Films> FilmsList { get; set; } = new List<Films>();

        public PageViewModel PageViewModel { get; }

        public FilmFilterViewModel FilterViewModel { get; }
        public FilmSortViewModel SortViewModel { get; }

        public FilmsViewModel(IEnumerable<Films> films, PageViewModel pageViewModel, FilmSortViewModel sortViewModel, FilmFilterViewModel filterViewModel)
        {
            FilmsList = films;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortViewModel;
        }
    }
}
