using AdminApplication.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Project_C.ViewModels
{
    //geprobeerd 
    public class ProductEditModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "veld is verplicht")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "veld is verplicht")]
        [MinLength(10, ErrorMessage = "beschrijving is te kort")]
        public string Description { get; set; }


        [Required(ErrorMessage = "veld is verplicht")]
        public double Price { get; set; }


        [Required]
        public string Place { get; set; }


        //foto is hier nullable, anders moet de gebruiker altijd een bestand uploaden bij het bewerken.
        //Inheretance en het overriden van de IformFile property zonder [Required] was niet succesvol op meerdere manieren, daarom een aparte class.
        public IFormFile? Photo { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "veld is verplicht")]
        [MinLength(20, ErrorMessage = "link is te kort")]
        public string VideoLink { get; set; }


        //elk product heeft een lijst met meerdere leveranciers, voor nu kan die ook leeg zijn (Many to many)
        // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
        public List<Guid>? Stores { get; set; }

        public string ExistingPhotoPath { get; set; }
    }
}
