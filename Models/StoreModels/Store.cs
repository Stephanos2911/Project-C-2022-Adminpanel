using System.ComponentModel.DataAnnotations;

namespace Project_C.Models.StoreModels
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public string SiteLink { get; set; }
    }
}
