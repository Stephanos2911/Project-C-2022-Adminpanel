using Project_C.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace Project_C.Models.StoreModels
{
    public class Store
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public string SiteLink { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
