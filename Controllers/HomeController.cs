using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminApplication.Models;
using AdminApplication.ViewModels;
using System.Diagnostics;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace AdminApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingenvironment, IProductRepository productRepository)
        {

            _logger = logger;
            _productRepository = productRepository;
            hostingEnvironment = hostingenvironment;
        }

        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult Index()
        {
            var listOfAllProducts = _productRepository.GetAllProducts();
            return View(listOfAllProducts);
        }

        [HttpGet]
        public ViewResult Delete(int? id)
        {
            Product selectedProduct = _productRepository.GetProduct(id ?? 1);
            return View(selectedProduct);
        }


        //Deletes the product and redirects to index after confirmation has been asked
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            Product selectedProduct = _productRepository.GetProduct(id);
            if (selectedProduct != null)
            {
                if (selectedProduct.PhotoPath != null)
                {
                    DeletePicture(selectedProduct.PhotoPath);
                }
                _productRepository.DeleteProduct(selectedProduct.Id);
            }
            return RedirectToAction("Index");
        }


        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productRepository.GetProduct(id?? 1);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpGet]
        public ViewResult Edit(int id)
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
        public IActionResult Edit(ProductEditViewModel productChanges)
        {
            if (ModelState.IsValid)
            {
                Product product = _productRepository.GetProduct(productChanges.Id);
                product.Name = productChanges.Name;
                product.Description = productChanges.Description;
                product.Price = productChanges.Price;
                product.Place = productChanges.Place;
                product.VideoLink = productChanges.VideoLink;
                
                if(productChanges.Photo != null)
                {
                    if(productChanges.ExistingPhotoPath != null)
                    {
                        DeletePicture(productChanges.ExistingPhotoPath);
                    }
                    product.PhotoPath = ProcessUploadedFile(productChanges);
                }

                _productRepository.UpdateProduct(product);
                return RedirectToAction("Details", new { id = product.Id });
            }

            return RedirectToAction("Index");

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        //creates the product and adds to database 
        [HttpPost]
        public IActionResult Create(ProductCreateViewModel product)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(product);
                string correctVideoLink = product.VideoLink;
                Product newProduct = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Place = product.Place,
                    PhotoPath = uniqueFileName,
                    VideoLink = correctVideoLink
                };
                Product X = _productRepository.AddProduct(newProduct);
            }

            return RedirectToAction("Index");
        }

        private string ProcessUploadedFile(ProductCreateViewModel product)
        {
            string uniqueFileName = null;
            if (product.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.Photo.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }

        private void DeletePicture(string uniqueName)
        {
            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images" , uniqueName);
            System.IO.File.Delete(filePath);
        }

        private string ProcessYoutubeLink(string originalLink)
        {
            string processedLink = null;

            return processedLink;
        }
    }
}