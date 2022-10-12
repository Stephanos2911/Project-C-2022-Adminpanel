using System.ComponentModel.DataAnnotations;

namespace Project_C.Models.StoreModels
{
    public class Store
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LogoPath { get; set; }
        [Required]
        public string SiteLink { get; set; }
    }
}
