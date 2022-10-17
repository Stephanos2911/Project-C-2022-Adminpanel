using System.ComponentModel.DataAnnotations;

namespace AdminApplication.ViewModels
{
    public class ProductEditViewModel : ProductCreateViewModel
    {
        public string ExistingPhotoPath { get; set; }
    }
}
