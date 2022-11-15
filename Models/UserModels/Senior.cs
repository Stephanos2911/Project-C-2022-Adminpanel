using Microsoft.Build.Framework;

namespace Project_C.Models.UserModels
{
    public class Senior
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public int HouseNr { get; set; }

        public Guid ProductCart { get; set; }
    }
}
