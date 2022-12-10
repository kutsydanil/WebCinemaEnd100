using WebCinema.Enum;

namespace WebCinema.Models.SortViewModels
{
    public class CountryProductionSortViewModel
    {
        public SortState NameSort { get; } // значение для сортировки по имени
        public SortState Current { get; } // значение свойства, выбранного для сортировки

        public CountryProductionSortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            Current = sortOrder;
        }
    }
}
