
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

        // This action method checks if an employee is logged in. If not, it redirects to the LoginPage.
        public RedirectToActionResult CheckLogin()
        {
            // If no employee is currently logged in, redirect to the LoginPage.
            if (!CurrentEmployee.IsLoggedIn())
            {
                return RedirectToAction("LoginPage", "Access");
            }

            // If an employee is logged in, return null.
            return null;
        }

        // This action method retrieves all products from the database and returns them in a view.
        public IActionResult ProductIndex()
        {
            // Check if an employee is logged in.
            CheckLogin();

            // Return the ProductIndex view with all products from the database.
            return View(_context.Products);
        }

        // This action method handles GET requests to the EditProduct endpoint.
        [HttpGet]
        public IActionResult EditProduct(Guid id)
        {
            // Check if an employee is logged in.
            CheckLogin();

            // Retrieve the product to be edited from the database.
            Product selectedProduct = _context.Products.Find(id);

            // Create a new ProductEditModel object with the properties of the selected product.
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

            // Create a SelectList containing all possible values for the Place property and set the selected value to the current value of the selected product.
            ViewBag.Places = new SelectList(places, selectedProduct.PlaceAsString);

            // Return the EditProduct view with the ProductEditModel object.
            return View(productEditViewModel);
        }

        // This action method handles POST requests to the EditProduct endpoint.
        [HttpPost]
        public IActionResult EditProduct(ProductEditModel productChanges)
        {
            // Check if an employee is logged in.
            CheckLogin();

            // Retrieve the product to be updated from the database.
            Product productToBeUpdated = _context.Products.Find(productChanges.Id);

            // Update the product's Description and Price properties with the values from the form.
            productToBeUpdated.Description = productChanges.Description;
            productToBeUpdated.Price = productChanges.Price;

            // If the PlaceAsString property has changed, update the Place and PlaceAsString properties accordingly.
            if(productToBeUpdated.PlaceAsString != productChanges.PlaceAsString)
            {
                productToBeUpdated.PlaceAsString = productChanges.PlaceAsString;

                // Use a switch statement to set the Place property to the correct integer value based on the selected room.
                switch (productToBeUpdated.PlaceAsString)
                {
                    case "Keuken":
                        productToBeUpdated.Place = 1;
                        break;
                    case "Badkamer":
                        productToBeUpdated.Place = 2;
                        break;
                    case "Woonkamer":
                        productToBeUpdated.Place = 3;
                        break;
                    case "Slaapkamer":
                        productToBeUpdated.Place = 4;
                        break;
                    case "Gezondheid":
                        productToBeUpdated.Place = 5;
                        break;
                    case "Buiten":
                        productToBeUpdated.Place = 6;
                        break;
                }
            }

            // Update the product's Name property with the value from the form.
            productToBeUpdated.Name = productChanges.Name;

            // If the VideoLink property has changed, update it to the new link.
            if (productToBeUpdated.VideoLink != productChanges.VideoLink)
            {
                // Extract the video ID from the YouTube link and create a new embed link.
                productToBeUpdated.VideoLink = $"https://www.youtube.com/embed/{productChanges.VideoLink.Substring(32, 11)}";
            }

            // If a new photo has been uploaded, update the product's ProductImage property with the new image.
            if (productChanges.Photo != null)
            {
                productToBeUpdated.ProductImage = StoreController.ImagetoByte(productChanges.Photo);
            }

            // Save changes to the database and redirect to the ProductIndex page.
            _context.SaveChanges();
            return RedirectToAction("ProductIndex");
        }



        //Deletes the product and redirects to index after confirmation has been asked
        public IActionResult ConfirmDeleteProduct(Guid id)
        {
            CheckLogin();
            //finds the product in the database with the given id
            Product selectedProduct = _context.Products.Find(id);
            //removes the selected product from the database
            _context.Products.Remove(selectedProduct);
            //saves the changes to the database
            _context.SaveChanges();
            //redirects to the ProductIndex action in the same controller
            return RedirectToAction("ProductIndex");
        }

        public IActionResult ProductDetails(Guid id)
        {
            CheckLogin();
            if (id == null)
            {
                //returns 404 Not Found if the given id is null
                return NotFound();
            }

            //finds the product in the database with the given id
            var product = _context.Products.Find(id);
            if (product == null)
            {
                //returns 404 Not Found if the product with the given id is not found in the database
                return NotFound();
            }

            //returns the ProductDetails view with the found product as its model
            return View(product);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            CheckLogin();
            //creates an empty ProductViewModel object as the model for the AddProduct view
            ProductViewModel model = new ProductViewModel();
            //returns the AddProduct view with the empty model
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

            //set Product.Place to the corresponding Integer, so we can route all products of a room in UserApplication
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

            //add the new product to the database
            _context.Products.Add(newProduct);
            //saves the changes to the database
            _context.SaveChanges();
            //redirects to the ProductIndex action in the same controller
            return RedirectToAction("ProductIndex");

        }


        //returns an Icollection of stores, based on the Ids (GUID) from the dropdown menu in AddProduct.CSHTML
        private ICollection<Store> ProcessChosenStores(List<Guid> selectedStores)
        { 
            ICollection<Store> stores = new Collection<Store>();
            foreach (var storeId in selectedStores)
            {
                if(storeId != null)
                {
                    stores.Add(_context.Stores.Find(storeId));
                }
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
