﻿
using AdminApplication.Models;
using AdminApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
  

        public ProductController( ApplicationDbContext context)
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

            //Parse int to room name with Switch case
            productToBeUpdated.PlaceAsString = productChanges.PlaceAsString;
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult DeleteProduct(Guid id)
        {
            CheckLogin();
            Product selectedProduct = _context.Products.Find(id);
            return View(selectedProduct);

        }

        //Deletes the product and redirects to index after confirmation has been asked
        [HttpPost, ActionName("DeleteProduct")]
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

            Product newProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Place = product.Place,
                ProductImage = StoreController.ImagetoByte(product.Photo),
                //voor een Iframe is een embed link nodig. we verwachten dat de gebruiker de normale link in de url balk bovenin zal kopieren en plakken. 
                //hierom converten we de link zelf naar een echte embed link. Dit doen we door het ID van de video eruit te slicen.
                VideoLink = $"https://www.youtube.com/embed/{product.VideoLink.Substring(32, 11)}",
                Stores = product.Stores != null ? ProcessChosenStores(product.Stores) : null
            };

            switch (product.Place)
            {
                case 1:
                    newProduct.PlaceAsString = "Keuken";
                    break;
                case 2:
                    newProduct.PlaceAsString = "Badkamer";
                    break;
                case 3:
                    newProduct.PlaceAsString = "Woonkamer";
                    break;
                case 4:
                    newProduct.PlaceAsString = "Slaapkamer";
                    break;
                case 5:
                    newProduct.PlaceAsString = "Gezondheid";
                    break;
                case 6:
                    newProduct.PlaceAsString = "Buiten";
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


        //public static string ProcessUploadedFile(IFormFile image, string subfolder, IWebHostEnvironment hostingenvironment)
        //{
        //    string uniqueFileName = null;
        //    if (image != null)
        //    {
        //        string uploadsFolder = $"{hostingenvironment.WebRootPath}/images/{subfolder}/";
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            image.CopyTo(fileStream);
        //        }

        //    }

        //    return uniqueFileName;
        //}

    }
}
