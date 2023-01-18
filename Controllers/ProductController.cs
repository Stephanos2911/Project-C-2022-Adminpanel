
using AdminApplication.Models;
using AdminApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Project_C.Models;
using Project_C.Models.UserModels;
using Project_C.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Project_C.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _context;
        public List<string> places;

        public ProductController( ApplicationDbContext context)
        {
            _context = context;
            places = new List<string> { "Keuken", "Badkamer", "Woonkamer", "Slaapkamer", "Gezondheid", "Buiten" };
        }

        public RedirectToActionResult CheckLogin()
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                return RedirectToAction("LoginPage", "Access");
            }
            return null;
        }

        public IActionResult ProductIndex()
        {
            CheckLogin();
            return View(_context.Products);
            
        }

        [HttpGet]
        public IActionResult EditProduct(Guid id)
        {
            CheckLogin();
            Product selectedProduct = _context.Products.Find(id);
            ProductEditModel productEditViewModel = new ProductEditModel
            {
                Id = selectedProduct.Id,
                Name = selectedProduct.Name,
                Description = selectedProduct.Description,
                Price = selectedProduct.Price,
                Place = selectedProduct.Place,
                PlaceAsString = selectedProduct.PlaceAsString,
                VideoLink = selectedProduct.VideoLink
            };

            ViewBag.Places = new SelectList(places, selectedProduct.PlaceAsString);
            return View(productEditViewModel);

        }

        [HttpPost]
        public IActionResult EditProduct(ProductEditModel productChanges)
        {
            CheckLogin();

            //get product to be updated
            Product productToBeUpdated = _context.Products.Find(productChanges.Id);
            productToBeUpdated.Description = productChanges.Description;
            productToBeUpdated.Price = productChanges.Price;

            //check if the PlaceAsString is different, meaning a different option has been chosen for Place in the form.
            if(productToBeUpdated.PlaceAsString != productChanges.PlaceAsString)
            {
                //replace the string with the new string
                productToBeUpdated.PlaceAsString = productChanges.PlaceAsString;

                //Every room (as string) corresponds to an Integer, which is used as a RoomID in the UserApplication to query all Products of a certain room.
                //here we use a switch to set the Place(int) property to the correct integer of the room.
                switch (productChanges.Place)
                {
                    case 1:
                        productToBeUpdated.PlaceAsString = "Keuken";
                        break;
                    case 2:
                        productToBeUpdated.PlaceAsString = "Badkamer";
                        break;
                    case 3:
                        productToBeUpdated.PlaceAsString = "Woonkamer";
                        break;
                    case 4:
                        productToBeUpdated.PlaceAsString = "Slaapkamer";
                        break;
                    case 5:
                        productToBeUpdated.PlaceAsString = "Gezondheid";
                        break;
                    case 6:
                        productToBeUpdated.PlaceAsString = "Buiten";
                        break;
                }
            }

            productToBeUpdated.Place = productChanges.Place;
            productToBeUpdated.Name = productChanges.Name;

            //if the link entered in the edit form is unchanged, dont process the link.
            //voorkomt exception waarbij een al verwerkte link nog een keer verwerkt word.
            if (productToBeUpdated.VideoLink != productChanges.VideoLink)
            {
                productToBeUpdated.VideoLink = $"https://www.youtube.com/embed/{productChanges.VideoLink.Substring(32, 11)}";
            }

            //is there a file uploaded?
            if (productChanges.Photo != null)
            {
                //delete the old photo and assign the new path to the updated product object.
                productToBeUpdated.ProductImage = StoreController.ImagetoByte(productChanges.Photo);
            }

            _context.SaveChanges();
            return RedirectToAction("ProductIndex");
        }


        //Deletes the product and redirects to index after confirmation has been asked
        public IActionResult ConfirmDeleteProduct(Guid id)
        {
            CheckLogin();
            Product selectedProduct = _context.Products.Find(id);
            _context.Products.Remove(selectedProduct);
            _context.SaveChanges();

            return RedirectToAction("ProductIndex");
        }

        public IActionResult ProductDetails(Guid id)
        {
            CheckLogin();
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            CheckLogin();
            ProductViewModel model = new ProductViewModel();
            return View(model);           
        }

        //creates the product and adds to database 
        [HttpPost]
        public IActionResult AddProduct(ProductViewModel product)
        {
            CheckLogin();

            //populate empty Product object with the data from the ViewModel
            Product newProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                PlaceAsString = product.PlaceAsString,
                ProductImage = StoreController.ImagetoByte(product.Photo),
                //to use an Iframe in html, an embedded link from youtube is needed. To get this link, the user has to under go a couple steps on youtube.
                //To simplify the process, we slice the video-ID at the correct position to create the correct embed link.
                VideoLink = $"https://www.youtube.com/embed/{product.VideoLink.Substring(32, 11)}",
                Stores = product.Stores != null ? ProcessChosenStores(product.Stores) : null
            };

            //set Product.PlaceAsString to the corresponding name, so we can print the name of the room instead of the roomID.
            switch (newProduct.PlaceAsString)
            {
                case "Keuken":
                    newProduct.Place = 1;
                    break;
                case "Badkamer":
                    newProduct.Place = 2;
                    break;
                case "Woonkamer":
                    newProduct.Place = 3;
                    break;
                case "Slaapkamer":
                    newProduct.Place = 4;
                    break;
                case "Gezondheid":
                    newProduct.Place = 5;
                    break;
                case "Buiten":
                    newProduct.Place = 6;
                    break;
            }

            _context.Products.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("ProductIndex");

        }


        //returns an Icollection of stores, based on the Ids (GUID) from the dropdown menu in AddProduct.CSHTML
        private ICollection<Store> ProcessChosenStores(List<Guid> selectedStores)
        { 
            ICollection<Store> stores = new Collection<Store>();
            foreach (var storeId in selectedStores)
            {
                stores.Add(_context.Stores.Find(storeId));
            }
            return stores;
        }


        //[DISCONTINUED]
        //uploads an IformFile object to the WWWROOT folder. discontinued because of the use of two different projects,
        //which means uploading a store picture to this wwwroot will not have use since the UserApplication doesnt have acces to that.   
        public static string ProcessUploadedFile(IFormFile image, string subfolder, IWebHostEnvironment hostingenvironment)
        {
            string uniqueFileName = null;
            if (image != null)
            {
                string uploadsFolder = $"{hostingenvironment.WebRootPath}/images/{subfolder}/";
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }

    }
}
