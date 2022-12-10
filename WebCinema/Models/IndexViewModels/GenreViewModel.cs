using CinemaCore.Models;
using WebCinema.Models.FilterViewModels;
using WebCinema.Models.PageViewModels;
using WebCinema.Models.SortViewModels;

namespace WebCinema.Models.IndexViewModels
{
    public class GenreViewModel
    {
        public IEnumerable<Genres> GenresList { get; } = new List<Genres>();
        public PageViewModel PageViewModel { get; }
        public GenreFilterViewModel FilterViewModel { get; }
        public GenreSortViewModelcs SortViewModel { get; }

        public GenreViewModel(IEnumerable<Genres> genres, PageViewModel pageViewModel, GenreFilterViewModel filterViewModel, GenreSortViewModelcs sortViewModel)
        {
            GenresList = genres;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortViewModel;
        }
    }
}
