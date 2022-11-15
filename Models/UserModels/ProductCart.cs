using System.ComponentModel.DataAnnotations;

namespace Project_C.Models.UserModels
{
    public class ProductCart
    {
        [Required]
        public Guid Id { get; set; }

        public List<int> ProductList { get; set; } 
    }
}
