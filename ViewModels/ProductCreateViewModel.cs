using Project_C.Models.StoreModels;
using System.ComponentModel.DataAnnotations;

namespace AdminApplication.ViewModels
{
    public class ProductCreateViewModel
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "product name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "description is required")]
        [MinLength(10, ErrorMessage = "description is too short")]
        public string Description { get; set; }
        [Required(ErrorMessage = "price is required")]
        [Range(0.0, 1000.0,
            ErrorMessage = "invalid price, try using a '.' instead of a ','")]
        public Decimal Price { get; set; }
        [Required]
        public string Place { get; set; }

        [Required(ErrorMessage = "product must have a photo")]
        public IFormFile Photo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "product must have a video")]
        [MinLength(20, ErrorMessage = "link is too short")]
        public string VideoLink { get; set; }

        //elk product heeft een lijst met meerdere leveranciers, voor nu kan die ook leeg zijn (Many to many)
        // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
        public List<Store> AllStores { get; set; }

        public List<Guid>? Stores { get; set; }
    }
}
