using AdminApplication.Controllers;
using AdminApplication.Models;
using AdminApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Project_C.Models.ProductModels;
using Project_C.Models.StoreModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Project_C.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ProductController(ILogger<HomeController> logger, IWebHostEnvironment hostingenvironment, IProductRepository productRepository, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            _productRepository = productRepository;
            hostingEnvironment = hostingenvironment;
        }
        public IActionResult ProductIndex()
        {
            var listOfAllProducts = _productRepository.GetAllProducts();
            return View(listOfAllProducts);
        }

        [HttpGet]
        public ViewResult EditProduct(Guid id)
        {
            Product selectedProduct = _productRepository.GetProduct(id);
            ProductEditViewModel productEditViewModel = new ProductEditViewModel
            {
                Id = selectedProduct.Id,
                Name = selectedProduct.Name,
                Description = selectedProduct.Description,
                Price = selectedProduct.Price,
                Place = selectedProduct.Place,
                ExistingPhotoPath = selectedProduct.PhotoPath,
                VideoLink = selectedProduct.VideoLink
            };
            return View(productEditViewModel);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductEditViewModel productChanges)
        {
            //get product to be updated
            Product productToBeUpdated = _productRepository.GetProduct(productChanges.Id);
            productToBeUpdated.Description = productChanges.Description;
            productToBeUpdated.Price = productChanges.Price;
            productToBeUpdated.Place = productChanges.Place;
            productToBeUpdated.Name = productChanges.Name;
            productToBeUpdated.VideoLink = productChanges.VideoLink;

            //is there a file uploaded?
            if (productChanges.Photo != null)
            {
                //is there an already existing photo? if yes then delete the old photo and assign the new path to the updated product object.
                if (productChanges.ExistingPhotoPath != null)
                {
                    DeletePicture(productChanges.ExistingPhotoPath, "ProductImages");
                }
                productToBeUpdated.PhotoPath = ProcessUploadedFile(productChanges.Photo, "ProductImages");
            }
            //no file uploaded means use the old photo
            else
            {
                productToBeUpdated.PhotoPath = productChanges.ExistingPhotoPath;
            }

            _productRepository.UpdateProduct(productToBeUpdated);
            return RedirectToAction("ProductIndex");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public ViewResult DeleteProduct(Guid id)
        {
            Product selectedProduct = _productRepository.GetProduct(id);
            return View(selectedProduct);
        }

        //Deletes the product and redirects to index after confirmation has been asked
        [HttpPost, ActionName("DeleteProduct")]
        public IActionResult ConfirmDeleteProduct(Guid id)
        {
            Product selectedProduct = _productRepository.GetProduct(id);
            if (selectedProduct != null)
            {
                if (selectedProduct.PhotoPath != null)
                {
                    DeletePicture(selectedProduct.PhotoPath, "ProductImages");
                }
                _productRepository.DeleteProduct(selectedProduct.Id);
            }
            return RedirectToAction("ProductIndex");
        }

        public IActionResult ProductDetails(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public ViewResult AddProduct()
        {
            ProductCreateViewModel model = new ProductCreateViewModel()
            {
                AllStores = _context.Stores.ToList()
            };
            return View(model);
        }

        //creates the product and adds to database 
        [HttpPost]
        public IActionResult AddProduct(ProductCreateViewModel product)
        {
            string uniqueFileName = ProcessUploadedFile(product.Photo, "ProductImages");
            string correctVideoLink = product.VideoLink;
            Product newProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Place = product.Place,
                PhotoPath = uniqueFileName,
                VideoLink = correctVideoLink,
                Stores = ProcessChosenStores(product.Stores)

            };

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
