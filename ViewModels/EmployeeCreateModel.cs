using System.ComponentModel.DataAnnotations;

namespace Project_C.ViewModels
{
    public class EmployeeCreateModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter a username")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter a password")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter an email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string IsAdmin { get; set; }
    }
}
