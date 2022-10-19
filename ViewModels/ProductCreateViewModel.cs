using System.ComponentModel.DataAnnotations;

namespace AdminApplication.ViewModels
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "product name is required")]
        [MinLength(1, ErrorMessage = "name is too short or too long")]
        public string Name { get; set; }
        [Required(ErrorMessage = "description is required")]
        [MinLength(10, ErrorMessage = "description is too short")]
        public string Description { get; set; }
        [Required(ErrorMessage = "price is required")]
        [Range(0.0, 1000.0,
            ErrorMessage = "invalid price, try using a '.' instead of a ','")]
        public float Price { get; set; }
        [Required]
        public string Place { get; set; }

        [Required(ErrorMessage = "product must have a photo")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "product must have a video")]
        [MinLength(5, ErrorMessage = "link is too short")]
        public string VideoLink { get; set; }


    }
}
