using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpMeNow.Shared
{
    public class UserRegister
    {
        [Required, EmailAddress(ErrorMessage ="Musisz podać adres Email")]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(50, MinimumLength = 6, ErrorMessage = "Hasło musi się składać z minimum 6 znaków")]
        public string Password { get; set; } = string.Empty;
       
        [Compare("Password", ErrorMessage = "Hasła nie są takie same")]
        public string Confirm { get; set; } = string.Empty;




    }
}
