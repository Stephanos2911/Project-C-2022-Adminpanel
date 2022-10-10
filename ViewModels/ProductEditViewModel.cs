using System.ComponentModel.DataAnnotations;

namespace Project_C.ViewModels
{
    public class ProductEditViewModel : ProductCreateViewModel
    {
        public int Id { get; set; }

        public string ExistingPhotoPath { get; set; }



    }
}
