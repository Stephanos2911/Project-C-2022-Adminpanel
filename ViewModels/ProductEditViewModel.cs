using System.ComponentModel.DataAnnotations;

namespace AdminApplication.ViewModels
{
    public class ProductEditViewModel : ProductCreateViewModel
    {
        public int Id { get; set; }

        public string ExistingPhotoPath { get; set; }



    }
}
