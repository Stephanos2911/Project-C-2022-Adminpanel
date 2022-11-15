using System.ComponentModel.DataAnnotations;

namespace Project_C.Models.UserModels
{
    public class Employee : User
    {
        [Required]
        public bool IsAdmin { get; set; }
    }
}
