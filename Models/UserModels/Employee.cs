using System.ComponentModel.DataAnnotations;

namespace Project_C.Models.UserModels
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<Message>? Messages { get; set; }
    }
}
