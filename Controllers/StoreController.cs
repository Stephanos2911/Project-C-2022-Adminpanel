
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

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public RedirectToActionResult CheckLogin()
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                return RedirectToAction("LoginPage", "Access");
            }
            return null;
        }

        public IActionResult StoreIndex()
        {
            CheckLogin();
            return View(_context.Stores);
        }


        [HttpGet]
        public IActionResult EditStore(Guid id)
        {
            CheckLogin();
            Store selectedStore = _context.Stores.Find(id);
            StoreEditModel StoreViewModel = new StoreEditModel
            {
                Id = id,
                Name = selectedStore.Name,
                SiteLink = selectedStore.SiteLink,
            };
            return View(StoreViewModel);
        }

        [HttpPost]
        public IActionResult EditStore(StoreEditModel storeChanges)
        {
            CheckLogin();
            //get product to be updated
            Store storeToBeUpdated = _context.Stores.Find(storeChanges.Id);
            storeToBeUpdated.SiteLink = storeChanges.SiteLink;
            storeToBeUpdated.Name = storeChanges.Name;

            //is there a file uploaded? if yes then change the current image
            if (storeChanges.LogoFile != null)
            {
                
                storeToBeUpdated.StoreLogo = ImagetoByte(storeChanges.LogoFile);
            }
            _context.SaveChanges();
            return RedirectToAction("StoreIndex");
        }

        [HttpGet]
        public IActionResult AddStore()
        {
            CheckLogin();
            return View();
        }



        [HttpPost]
        public IActionResult AddStore(StoreViewModel store)
        {
            CheckLogin();
            Store newStore = new Store
            {
                Id = store.Id,
                Name = store.Name,
                SiteLink = store.SiteLink,
                StoreLogo = ImagetoByte(store.LogoFile)
            };
            _context.Stores.Add(newStore);
            _context.SaveChanges();
            return RedirectToAction("StoreIndex");
        }

        //here an IformFile object (a file that is uploaded) is converted into a Byte array using a MemoryStream. 
        //this array is saved into the database as a Blob and converted into an image in the Views.
        public static byte[] ImagetoByte(IFormFile logoFile)
        {
            MemoryStream ms = new MemoryStream();
            logoFile.CopyTo(ms);
            return ms.ToArray();
        }

        public IActionResult StoreDetails(Guid id)
        {
            CheckLogin();   
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
            CheckLogin();
            Store selectedStore = _context.Stores.Find(id);
            return View(selectedStore);

        }

        //Deletes the product and redirects to index after confirmation has been asked
        [HttpPost,ActionName("DeleteStore")]
        public IActionResult ConfirmDeleteStore(Guid id)
        {
            CheckLogin();
            Store selectedStore = _context.Stores.Find(id);
            if (selectedStore != null)
            {
                _context.Stores.Remove(selectedStore);
                _context.SaveChanges();
            }
            return RedirectToAction("StoreIndex");
        }

    }
}
