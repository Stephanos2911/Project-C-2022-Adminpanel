using Microsoft.Build.Framework;

namespace AdminApplication.ViewModels
{
    public class StoreCreateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile LogoFile { get; set; }
        [Required]
        public string SiteLink { get; set; }
    }
}
