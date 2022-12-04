using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project_C.ViewModels
{
    public class EmployeeCreateModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "veld is verplicht")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "veld is verplicht")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "veld is verplicht")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string IsAdmin { get; set; }
    }
}
