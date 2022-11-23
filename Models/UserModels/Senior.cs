using System.ComponentModel.DataAnnotations;
using Project_C.Models.ProductModels;

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

        //een senior heeft een cart met meerdere producten, die leeg kan zijn (One to Many) :
        //https://learn.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
        public ICollection<Product>? Products { get; set; }

        //iedere senior heeft 1 mantelzorger (one to many)
        //public Guid? CareTakerId { get; set; }
        //public CareTaker? CareTaker { get; set; }

    }
}
