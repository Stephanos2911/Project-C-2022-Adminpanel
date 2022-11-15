using System.ComponentModel.DataAnnotations;

namespace Project_C.Models.UserModels
{
    public class Employee
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
    }
}
