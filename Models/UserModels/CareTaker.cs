using Microsoft.Build.Framework;

namespace Project_C.Models.UserModels
{
    public class CareTaker
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public List<Guid> SeniorId { get; set; }

    }
}
