using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace WebCinema.Models.FilterViewModels
{
    public class CountryProductionFilterViewModel
    {
        public CountryProductionFilterViewModel(string? country) 
        {
            SelectedCountry = country;
        }

        public string? SelectedCountry { get; }
    }
}
