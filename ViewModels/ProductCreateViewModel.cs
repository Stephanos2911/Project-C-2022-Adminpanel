using System.ComponentModel.DataAnnotations;

namespace AdminApplication.ViewModels
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Place { get; set; }

        public IFormFile Photo { get; set; }

        public string VideoLink { get; set; }


    }
}
