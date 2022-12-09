using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;


namespace WebCinema.ViewModel.Users
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Не указан электронный адрес")]
        [EmailAddress(ErrorMessage = "Некорректный электронный адрес")]
        [Display(Name = "Электронный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        //[DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
