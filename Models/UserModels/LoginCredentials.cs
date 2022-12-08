using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project_C.Models.UserModels
{
    public class LoginCredentials
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter an email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
       
        public string Email { get; set; }

        //public string _emailAddress { get { return Email; } set { Email; } }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter a password")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
