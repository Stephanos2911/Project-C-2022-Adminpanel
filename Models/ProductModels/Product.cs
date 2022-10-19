using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Project_C.Models.ProductModels
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Place { get; set; }
        public string? PhotoPath { get; set; }
        public string? VideoLink { get; set; }
    }
}
