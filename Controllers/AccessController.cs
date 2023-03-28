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

    // This action method handles GET requests to the LoginPage endpoint.
    [HttpGet]
    public IActionResult LoginPage()
    {
        // Returns the Login view.
        return View();
    }

    // This action method handles GET requests to the HomePage endpoint.
    public IActionResult HomePage() 
    { 
        // Returns the Home view.
        return View(); 
    }

    // This action method handles POST requests to the LoginPage endpoint.
    [HttpPost]
    public IActionResult LoginPage(LoginCredentials logintry)
    {
        // Retrieves an employee from the database based on their email address.
        var employee = _context.Employees.FirstOrDefault(x => x.Email == logintry.Email);

        // If no employee is found, redirect back to the LoginPage.
        if (employee == null)
        {
            return RedirectToAction("LoginPage");
        }

        // If the employee's password matches the provided password and their email matches the provided email,
        // set the currentEmployee property to the retrieved employee and redirect to the HomePage.
        if (employee.Password == logintry.Password && employee.Email == logintry.Email)
        {
            CurrentEmployee.currentEmployee = employee;
            return RedirectToAction("HomePage");
        }

        // If the provided email and password do not match any employee in the database, redirect back to the LoginPage.
        return RedirectToAction("LoginPage");
    }

    // This action method handles GET requests to the Logout endpoint.
    public IActionResult Logout()
    {
        // Set the currentEmployee property to null and redirect to the LoginPage.
        CurrentEmployee.currentEmployee = null;
        return RedirectToAction("LoginPage");
    }

    }
}
