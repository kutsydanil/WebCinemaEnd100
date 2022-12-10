using WebCinema.Enum;

namespace WebCinema.Models.SortViewModels
{
    public class FilmProductionSortViewModel
    {
        public SortState NameSort { get; }

        public SortState CountryNameSort { get; }

        public SortState Current { get; }

        public FilmProductionSortViewModel(SortState sortOrder) 
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            CountryNameSort = sortOrder == SortState.CountryNameAsc ? SortState.CountryNameDesc : SortState.CountryNameAsc;
            Current = sortOrder;
        }
    }
}
