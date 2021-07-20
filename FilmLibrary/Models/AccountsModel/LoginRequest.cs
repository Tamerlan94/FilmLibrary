using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
