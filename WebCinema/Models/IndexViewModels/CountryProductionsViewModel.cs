using CinemaCore.Models;
using WebCinema.Enum;
using WebCinema.Models.FilterViewModels;
using WebCinema.Models.PageViewModels;
using WebCinema.Models.SortViewModels;

namespace WebCinema.Models.IndexViewModels
{
    public class CountryProductionsViewModel
    {
        public IEnumerable<CountryProductions> CountryProductionsList { get; } = new List<CountryProductions>();

        public CountryProductionSortViewModel SortViewModel { get; }

        public CountryProductionFilterViewModel FilterViewModel { get; }

        public PageViewModel PageViewModel { get; }

        public CountryProductionsViewModel(IEnumerable<CountryProductions> countryProductions, PageViewModel pageviewModel,
            CountryProductionFilterViewModel filterViewModel, CountryProductionSortViewModel sortViewModel)
        {
            CountryProductionsList = countryProductions;
            PageViewModel = pageviewModel;
            SortViewModel = sortViewModel;
            FilterViewModel = filterViewModel;
        }


    }
}
