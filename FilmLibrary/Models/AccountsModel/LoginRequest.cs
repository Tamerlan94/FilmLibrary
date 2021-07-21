using System.ComponentModel.DataAnnotations;

namespace FilmLibrary.Models.AccountsModel
{
    public class LoginRequest
    {
        [Required] [Display(Name = "Email")]
        public string Email { get; set; }

        [Required] [DataType(DataType.Password)] [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
