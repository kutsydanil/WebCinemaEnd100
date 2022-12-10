namespace WebCinema.Models.FilterViewModels
{
    public class FilmProductionFilterViewModel
    {
        public string? SelectedName { get; }

        public FilmProductionFilterViewModel(string? filmProductionName)
        {
            SelectedName = filmProductionName;
        }
    }
}
