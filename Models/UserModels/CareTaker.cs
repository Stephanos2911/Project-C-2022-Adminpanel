using System.ComponentModel.DataAnnotations;

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

        //iedere mantelzorger kan meerdere senioren hebben, iedere senior heeft 1 mantelzorger (One to many)
        //https://www.learnentityframeworkcore.com/configuration/one-to-many-relationship-configuration
        public ICollection<Senior>? Seniors { get; set; }

    }
}
