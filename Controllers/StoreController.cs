using AdminApplication.Controllers;
using AdminApplication.Models;
using AdminApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Project_C.Models.StoreModels;
using System.Diagnostics;

namespace Project_C.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoreRepository _storeRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public StoreController(ILogger<HomeController> logger, IWebHostEnvironment hostingenvironment, IStoreRepository storeRepository)
        {

            _logger = logger;
            _storeRepository = storeRepository;
            hostingEnvironment = hostingenvironment;
        }

        public IActionResult StoreIndex()
        {
            var listOfAllProducts = _storeRepository.GetAllStores();
            return View(listOfAllProducts);
        }


        [HttpGet]
        public ViewResult EditStore(int? id)
        {
            Store selectedStore = _storeRepository.GetStore(id ?? 1);
            StoreEditViewModel storeEditViewModel = new StoreEditViewModel
            {
                Id = selectedStore.Id,
                Name = selectedStore.Name,
                ExistingPhotoPath = selectedStore.LogoPath,
                SiteLink = selectedStore.SiteLink,
            };
            return View(storeEditViewModel);
        }

        [HttpPost]
        public IActionResult EditStore(StoreEditViewModel storeChanges)
        {
            //get product to be updated
            Store storeToBeUpdated = _storeRepository.GetStore(storeChanges.Id);
            storeToBeUpdated.SiteLink = storeChanges.SiteLink;
            storeToBeUpdated.Name = storeChanges.Name;

            //is there a file uploaded?
            if (storeChanges.LogoFile != null)
            {
                //is there an already existing photo? if yes then delete the old photo and assign the new path to the updated product object.
                if (storeChanges.ExistingPhotoPath != null)
                {
                    DeletePicture(storeChanges.ExistingPhotoPath, "ProductImages");
                }
                storeToBeUpdated.LogoPath = ProcessUploadedFile(storeChanges.LogoFile, "ProductImages");
            }
            //no file uploaded means use the old photo
            else
            {
                storeToBeUpdated.LogoPath = storeChanges.ExistingPhotoPath;
            }

            _storeRepository.UpdateStore(storeToBeUpdated);
            return RedirectToAction("StoreIndex");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string ProcessUploadedFile(IFormFile image, string subfolder)
        {
            string uniqueFileName = null;
            if (image != null)
            {
                string uploadsFolder = $"{hostingEnvironment.WebRootPath}/images/{subfolder}/";
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }

        [HttpGet]
        public IActionResult AddStore()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddStore(StoreCreateViewModel store)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(store.LogoFile, "StoreLogos");
                Store newStore = new Store
                {
                    Id = store.Id,
                    Name = store.Name,
                    SiteLink = store.SiteLink,
                    LogoPath = uniqueFileName,
                };
                _storeRepository.AddStore(newStore);
            }

            return RedirectToAction("StoreIndex");
        }

        public IActionResult StoreDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = _storeRepository.GetStore(id ?? 1);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        [HttpGet]
        public ViewResult DeleteStore(int? id)
        {
            Store selectedStore = _storeRepository.GetStore(id ?? 1);
            return View(selectedStore);
        }

        //Deletes the product and redirects to index after confirmation has been asked
        [HttpPost, ActionName("DeleteStore")]
        public IActionResult DeleteStore(int id)
        {
            Store selectedStore = _storeRepository.GetStore(id);
            if (selectedStore != null)
            {
                if (selectedStore.LogoPath != null)
                {
                    DeletePicture(selectedStore.LogoPath, "StoreLogos");
                }
                _storeRepository.DeleteStore(selectedStore.Id);
            }
            return RedirectToAction("StoreIndex");
        }

        private void DeletePicture(string uniqueImageName, string subfolder)
        {
            string filePath = $"{hostingEnvironment.WebRootPath}/images/{subfolder}/{uniqueImageName}";
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }


    }
}
