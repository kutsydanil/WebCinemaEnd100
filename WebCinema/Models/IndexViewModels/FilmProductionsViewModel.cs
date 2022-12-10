using CinemaCore.Models;
using WebCinema.Models.FilterViewModels;
using WebCinema.Models.PageViewModels;
using WebCinema.Models.SortViewModels;

namespace WebCinema.Models.IndexViewModels
{
    public class FilmProductionsViewModel
    {
        public IEnumerable<FilmProductions> FilmProductionsList { get;} = new List<FilmProductions>();
        
        public PageViewModel PageViewModel { get; }
        public FilmProductionFilterViewModel FilterViewModel { get; }
        public FilmProductionSortViewModel SortViewModel { get; }

        public FilmProductionsViewModel(IEnumerable<FilmProductions> filmProductions, PageViewModel pageViewModel, FilmProductionFilterViewModel filterViewModel, FilmProductionSortViewModel sortViewModel)
        {
            FilmProductionsList = filmProductions;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortViewModel;
        }
    }
}
