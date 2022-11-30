using AdminApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Project_C.Models.UserModels;

namespace Project_C.Controllers
{
    public class AccessController : Controller
    {
        private ApplicationDbContext _context;
        public AccessController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult HomePage() { return View(); }

        [HttpPost]
        public IActionResult LoginPage(LoginCredentials logintry)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.Email == logintry.Email);
            if (employee == null)
            {
                return RedirectToAction("LoginPage");
            }
            if (employee.Password == logintry.Password && employee.Email == logintry.Email)
            {
                CurrentEmployee.currentEmployee = employee;
                return RedirectToAction("HomePage");
            }
            return RedirectToAction("LoginPage");
        }
        public IActionResult Logout()
        {
            CurrentEmployee.currentEmployee = null;
            return RedirectToAction("LoginPage");
        }

    }
}
