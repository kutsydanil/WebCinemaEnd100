using WebCinema.Enum;

namespace WebCinema.Models.SortViewModels
{
    public class GenreSortViewModelcs
    {
        public SortState Current { get; }
        public SortState NameSort { get; }

        public GenreSortViewModelcs(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            Current = sortOrder;
        }

    }
}
