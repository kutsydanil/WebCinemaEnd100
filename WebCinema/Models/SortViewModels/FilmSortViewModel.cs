using WebCinema.Enum;

namespace WebCinema.Models.SortViewModels
{
    public class FilmSortViewModel
    {
        public SortState NameSort { get; }
        public SortState CountryProductionSort { get; }
        public SortState FilmProductionSort { get; }
        public SortState AgeSort { get; }
        public SortState GenreSort { get; } 
        public SortState DurationSort { get; }
        public SortState Current { get; }

        public FilmSortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            CountryProductionSort = sortOrder == SortState.CountryProductionAsc ? SortState.CountryProductionDesc : SortState.CountryProductionAsc;
            FilmProductionSort = sortOrder == SortState.FilmProductionAsc ? SortState.FilmProductionDesc : SortState.FilmProductionAsc;
            AgeSort = sortOrder == SortState.AgeAsc ? SortState.AgeDesc : SortState.AgeAsc;
            GenreSort = sortOrder == SortState.GenreAsc ? SortState.GenreDesc : SortState.GenreAsc;
            DurationSort = sortOrder == SortState.DurationAsc ? SortState.DurationDesc : SortState.DurationAsc;
            Current = sortOrder;
        }
    }
}
