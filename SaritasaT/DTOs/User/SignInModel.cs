using System.ComponentModel.DataAnnotations;

namespace SaritasaT.DTOs.User
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Email required")]
        [RegularExpression(@"^([\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+\.)*[\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Email address must be a valid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}
