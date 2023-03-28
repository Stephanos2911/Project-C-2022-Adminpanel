
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
            // check if the user is logged in
            if (!CurrentEmployee.IsLoggedIn())
            {
                // if not, redirect to the login page
                return RedirectToAction("LoginPage", "Access");
            }
            // if user is logged in, return null
            return null;
        }

        public IActionResult StoreIndex()
        {
            // check if user is logged in
            CheckLogin();
            // retrieve all stores from the database and pass them to the view
            return View(_context.Stores);
        }

        [HttpGet]
        public IActionResult EditStore(Guid id)
        {
            // check if user is logged in
            CheckLogin();
            // retrieve the selected store from the database
            Store selectedStore = _context.Stores.Find(id);
            // create a view model to store the store's data for editing
            StoreEditModel StoreViewModel = new StoreEditModel
            {
                Id = id,
                Name = selectedStore.Name,
                SiteLink = selectedStore.SiteLink,
            };
            // pass the view model to the view for editing
            return View(StoreViewModel);
        }

        [HttpPost]
        public IActionResult EditStore(StoreEditModel storeChanges)
        {
            // check if user is logged in
            CheckLogin();
            // retrieve the store to be updated from the database
            Store storeToBeUpdated = _context.Stores.Find(storeChanges.Id);
            // update the store's properties with the new values from the view model
            storeToBeUpdated.SiteLink = storeChanges.SiteLink;
            storeToBeUpdated.Name = storeChanges.Name;
            // check if a new store logo file was uploaded, and update the store's logo if so
            if (storeChanges.LogoFile != null)
            {
                storeToBeUpdated.StoreLogo = ImagetoByte(storeChanges.LogoFile);
            }
            // save the changes to the database
            _context.SaveChanges();
            // redirect back to the StoreIndex page
            return RedirectToAction("StoreIndex");
        }

        [HttpGet]
        public IActionResult AddStore()
        {
            // check if user is logged in
            CheckLogin();
            // return the AddStore view
            return View();
        }

        [HttpPost]
        public IActionResult AddStore(StoreViewModel store)
        {
            // check if user is logged in
            CheckLogin();
            // create a new Store object and populate its properties with the values from the view model
            Store newStore = new Store
            {
                Id = store.Id,
                Name = store.Name,
                SiteLink = store.SiteLink,
                StoreLogo = ImagetoByte(store.LogoFile)
            };
            // add the new store to the database
            _context.Stores.Add(newStore);
            // save the changes to the database
            _context.SaveChanges();
            // redirect back to the StoreIndex page
            return RedirectToAction("StoreIndex");
        }


        // The ImagetoByte function takes an IFormFile object (a file that is uploaded) and converts it into a byte array.
        // A MemoryStream is used to read the data from the IFormFile and convert it into a byte array, which is returned.
        // This byte array is then saved into the database as a Blob and converted into an image in the Views.

        public static byte[] ImagetoByte(IFormFile logoFile)
        {
            MemoryStream ms = new MemoryStream();
            logoFile.CopyTo(ms);
            return ms.ToArray();
        }

        // The StoreDetails action method displays the details of a store with the given id.
        // If no id is provided or if the store with the given id is not found, a 404 Not Found status code is returned.
        // Otherwise, the StoreDetails view is returned with the details of the store.
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

        // The DeleteStore action method displays the selected store for confirmation of deletion.
        // If the store with the given id is not found, a 404 Not Found status code is returned.
        // Otherwise, the DeleteStore view is returned with the selected store.
        [HttpGet]
        public IActionResult DeleteStore(Guid id)
        {
            CheckLogin();
            Store selectedStore = _context.Stores.Find(id);
            return View(selectedStore);
        }

        // The ConfirmDeleteStore action method deletes the selected store after user confirmation and redirects to the StoreIndex view.
        // If the store with the given id is not found, no action is taken.
        // Otherwise, the store is removed from the database and the StoreIndex view is returned.
        [HttpPost, ActionName("DeleteStore")]
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
