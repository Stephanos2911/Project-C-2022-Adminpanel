using Microsoft.AspNetCore.Mvc;
using AdminApplication.Models;
using AdminApplication.ViewModels;
using System.Diagnostics;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Project_C.Models.StoreModels;

namespace AdminApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment hostingEnvironment;
        

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingenvironment,
            IStoreRepository storerepository)
        {

            _logger = logger;
            hostingEnvironment = hostingenvironment;
        }


        public IActionResult HomePage()
        {
            return View();
        }


        private string ProcessYoutubeLink(string originalLink)
        {
            string processedLink = null;

            return processedLink;
        }
    }
}