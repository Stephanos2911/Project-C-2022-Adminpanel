using AdminApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Project_C.Models.UserModels;
using Project_C.ViewModels;

namespace Project_C.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context) { 
            _context= context;
        }

        public RedirectToActionResult CheckLogin()
        {
            //if (!CurrentEmployee.IsLoggedIn())
            //{
            //    return RedirectToAction("LoginPage", "Access");
            //}
            //if (!CurrentEmployee.IsAdmin())
            //{
            //    return RedirectToAction("HomePage", "Access");
            //}
            return null;
        }

        // Controller action for the employee index page
        public IActionResult EmployeeIndex()
        {
            CheckLogin(); // Check if the user is logged in
            return View(_context.Employees); // Display a view of all employees
        }

        // Controller action for the add employee page (HTTP GET)
        [HttpGet]
        public IActionResult AddEmployee()
        {
            CheckLogin(); // Check if the user is logged in
            return View(); // Display a view for adding a new employee
        }

        // Controller action for adding a new employee to the database (HTTP POST)
        [HttpPost]
        public IActionResult AddEmployee(EmployeeCreateModel newEmployeemodel)
        {
            CheckLogin(); // Check if the user is logged in
            if (ModelState.IsValid) // Check if the data model is valid
            {
                // Create a new employee object with the data from the form
                Employee newEmployee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    Username = newEmployeemodel.Username,
                    Password = newEmployeemodel.Password,
                    Email = newEmployeemodel.Email,
                    IsAdmin = newEmployeemodel.IsAdmin == "Administrator" ? true : false
                };

                // Add the new employee to the database and save changes
                _context.Employees.Add(newEmployee);
                _context.SaveChanges();
            }

            // Redirect to the employee index page
            return RedirectToAction("EmployeeIndex");
        }

        // Controller action for the employee details page
        public IActionResult EmployeeDetails(Guid id)
        {
            CheckLogin(); // Check if the user is logged in
            var currentEmployee = _context.Employees.Find(id); // Find the employee with the specified ID
            if (currentEmployee == null) // If the employee doesn't exist, return a 404 error
            {
                return NotFound();
            }

            // Display a view of the employee's details
            return View(currentEmployee);
        }

        // Controller action for the delete employee page (HTTP GET)
        [HttpGet]
        public IActionResult DeleteEmployee(Guid id)
        {
            CheckLogin(); // Check if the user is logged in
            return View(_context.Employees.Find(id)); // Display a view for deleting the specified employee
        }

        // Controller action for deleting an employee from the database (HTTP POST)
        [HttpPost, ActionName("DeleteEmployee")]
        public IActionResult ConfirmDeleteEmployee(Guid id)
        {
            CheckLogin(); // Check if the user is logged in
            _context.Employees.Remove(_context.Employees.Find(id)); // Remove the employee with the specified ID from the database
            _context.SaveChanges(); // Save changes to the database
            return RedirectToAction("EmployeeIndex"); // Redirect to the employee index page
        }



        // Finds the employee with the specified id and creates a new EmployeeCreateModel based on it, 
        // which will be used to modify the employee.
        [HttpGet]
        public IActionResult EditEmployee(Guid id)
        {
            CheckLogin();
            // Find employee by id.
            Employee currentEmployee = _context.Employees.Find(id);

            // Create new EmployeeCreateModel with data from current employee.
            EmployeeCreateModel employeeCreateModel = new EmployeeCreateModel()
            {
                Id = currentEmployee.Id,
                Username = currentEmployee.Username,
                Password = currentEmployee.Password,
                Email = currentEmployee.Email,
                IsAdmin = currentEmployee.IsAdmin == true ? "Administrator" : "Medewerker"
            };

            // Return the view with the new EmployeeCreateModel.
            return View(employeeCreateModel);
        }


        // Updates the employee with the data from the submitted EmployeeCreateModel.
        [HttpPost]
        public IActionResult EditEmployee(EmployeeCreateModel employeechanges)
        {
            CheckLogin();
            if (ModelState.IsValid)
            {
                // Find employee in database by id.
                Employee employeeToBeUpdated = _context.Employees.Find(employeechanges.Id);

                // Modify the employee with the data from the submitted EmployeeCreateModel.
                employeeToBeUpdated.Username = employeechanges.Username;
                employeeToBeUpdated.Password = employeechanges.Password;
                employeeToBeUpdated.Email = employeechanges.Email;
                employeeToBeUpdated.IsAdmin = employeechanges.IsAdmin == "Administrator" ? true : false;

                // Save the changes to the employee record in the database.
                _context.SaveChanges();

                // Redirect to the EmployeeIndex page.
                return RedirectToAction("EmployeeIndex");
            }

            // If ModelState is not valid, redirect to the EmployeeIndex page.
            return RedirectToAction("EmployeeIndex");
        }

    }
}
