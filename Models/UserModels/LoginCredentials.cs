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
        public string Password { get; set; }
    }
}
