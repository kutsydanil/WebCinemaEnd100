using WebCinema.Enum;

namespace WebCinema.Models.SortViewModels
{
    public class FilmProductionSortViewModel
    {
        public SortState NameSort { get; }

        public SortState Current { get; }

        public FilmProductionSortViewModel(SortState sortOrder) 
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            Current = sortOrder;
        }
    }
}
