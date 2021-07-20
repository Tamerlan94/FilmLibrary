using System;
using System.ComponentModel.DataAnnotations;

namespace FilmLibrary.Models.AccountsModel
{
    public class RegisterRequest
    {
        [Required] [Display(Name = "Email")]
        public string Email { get; set; }

        [Required] [DataType(DataType.Password)] [Display(Name = "Password")]
        public string Password { get; set; }

        [Required] [Compare("Password", ErrorMessage = "Password not same")] [DataType(DataType.Password)] [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Name")]  
        public string Name { get; set; }

        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Display(Name = "Birthdate")]
        public DateTime Birthdate { get; set; }

        ///
        //Если ты хочешь использовать отображаемые описания каждого поля отдельно в html, то поставь в комменты [Display] для каждого поля
        ///
    }
}
