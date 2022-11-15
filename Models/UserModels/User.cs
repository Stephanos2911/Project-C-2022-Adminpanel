using Microsoft.Build.Framework;

namespace Project_C.Models.UserModels
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
