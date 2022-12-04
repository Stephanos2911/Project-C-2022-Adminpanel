
using AdminApplication.Models;
using AdminApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Project_C.Models;
using Project_C.Models.UserModels;
using Project_C.ViewModels;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Project_C.Controllers
{
    public class StoreController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public StoreController(IWebHostEnvironment hostingenvironment, ApplicationDbContext context)
        {
            _context = context;
            hostingEnvironment = hostingenvironment;
        }

        public IActionResult DirectToLogin()
        {
            return RedirectToAction("LoginPage", "Access");

        }

        public IActionResult StoreIndex()
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                 return DirectToLogin();
            }
            
            return View(_context.Stores);
        }


        [HttpGet]
        public IActionResult EditStore(Guid id)
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                 return DirectToLogin();
            }
            Store selectedStore = _context.Stores.Find(id);
            StoreEditModel StoreViewModel = new StoreEditModel
            {
                Id = id,
                Name = selectedStore.Name,
                ExistingPhotoPath = selectedStore.LogoPath,
                SiteLink = selectedStore.SiteLink,
            };
            return View(StoreViewModel);
        }

        [HttpPost]
        public IActionResult EditStore(StoreEditModel storeChanges)
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                 return DirectToLogin();
            }
            //get product to be updated
            Store storeToBeUpdated = _context.Stores.Find(storeChanges.Id);
            storeToBeUpdated.SiteLink = storeChanges.SiteLink;
            storeToBeUpdated.Name = storeChanges.Name;

            //is there a file uploaded? if yes then continue
            if (storeChanges.LogoFile != null)
            {
                //is there an already existing photo? if yes then delete the old photo and assign the new path to the updated product object.
                if (storeChanges.ExistingPhotoPath != null)
                {
                    DeletePicture(storeChanges.ExistingPhotoPath);
                }
                storeToBeUpdated.LogoPath = ProductController.ProcessUploadedFile(storeChanges.LogoFile, "StoreLogos", hostingEnvironment);
            }
            //no file uploaded means use the old photo
            else
            {
                storeToBeUpdated.LogoPath = storeChanges.ExistingPhotoPath;
            }

            _context.SaveChanges();
            return RedirectToAction("StoreIndex");

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult AddStore()
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                 return DirectToLogin();
            }
            return View();
        }



        [HttpPost]
        public IActionResult AddStore(StoreViewModel store)
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                 return DirectToLogin();
            }
            string uniqueFileName = ProductController.ProcessUploadedFile(store.LogoFile, "StoreLogos", hostingEnvironment);
            Store newStore = new Store
            {
                Id = store.Id,
                Name = store.Name,
                SiteLink = store.SiteLink,
                LogoPath = uniqueFileName,
            };
            _context.Stores.Add(newStore);
            _context.SaveChanges();
            return RedirectToAction("StoreIndex");
        }

        public IActionResult StoreDetails(Guid id)
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                 return DirectToLogin();
            }
            if (id == null)
            {
                return NotFound();
            }

            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);

        }

        [HttpGet]
        public IActionResult DeleteStore(Guid id)
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                 return DirectToLogin();
            }
            Store selectedStore = _context.Stores.Find(id);
            return View(selectedStore);

        }

        //Deletes the product and redirects to index after confirmation has been asked
        [HttpPost,ActionName("DeleteStore")]
        public IActionResult ConfirmDeleteStore(Guid id)
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                 return DirectToLogin();
            }
            Store selectedStore = _context.Stores.Find(id);
            if (selectedStore != null)
            {
                if (selectedStore.LogoPath != null)
                {
                    DeletePicture(selectedStore.LogoPath);
                }
                _context.Stores.Remove(selectedStore);
                _context.SaveChanges();
            }
            return RedirectToAction("StoreIndex");
        }

        private void DeletePicture(string uniqueImageName)
        {
            string uploadsFolder = $"{hostingEnvironment.WebRootPath}/images/StoreLogos/";
            string filePath = Path.Combine(uploadsFolder, uniqueImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
