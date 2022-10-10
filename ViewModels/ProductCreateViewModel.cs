using System.ComponentModel.DataAnnotations;

namespace Project_C.ViewModels
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string Place { get; set; }

        public IFormFile Photo { get; set; }

        public string VideoLink { get; set; }


    }
}
