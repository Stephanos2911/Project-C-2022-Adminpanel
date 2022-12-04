using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace AdminApplication.ViewModels
{
    public class StoreViewModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "a name is required")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "name is too long")]

        public string Name { get; set; }
        [Required(ErrorMessage = "store must have a logo")]
        public IFormFile LogoFile { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "link to store website is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "link is too short")]

        public string SiteLink { get; set; }

    }
}
