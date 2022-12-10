namespace WebCinema.Models.FilterViewModels
{
    public class GenreFilterViewModel
    {
        public string? SelectedName { get; }

        public GenreFilterViewModel(string? genreName)
        {
            SelectedName = genreName;
        }
    }
}
