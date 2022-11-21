using AdminApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_C.Models.ProductModels;
using Project_C.Models.StoreModels;
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

        //Employee index page
        public IActionResult EmployeeIndex()
        {
            var AllEmployees = _context.Employees;
            return View(AllEmployees);
        }

        //Add Employee Page
        [HttpGet]
        public ViewResult AddEmployee() {
            return View();
        }

        //Adds employee to database and redirects to index 
        [HttpPost]
        public IActionResult AddEmployee(EmployeeCreateModel newEmployeemodel)
        {
            if (ModelState.IsValid)
            {
                Employee newEmployee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    Username = newEmployeemodel.Username,
                    Password = newEmployeemodel.Password,
                    Email = newEmployeemodel.Email,
                    IsAdmin = newEmployeemodel.IsAdmin == "Yes" ? true : false
                };

                _context.Employees.Add(newEmployee);
                _context.SaveChanges();
            }
            else
            {
                return RedirectToAction("AddEmployee");
            }

            return RedirectToAction("EmployeeIndex");
        }

        //Details page for employee
        public IActionResult EmployeeDetails(Guid employeeId)
        {
            var currentEmployee = _context.Employees.Find(employeeId);
            if (currentEmployee == null)
            {
                return NotFound();
            }

            return View(currentEmployee);
        }

        //directs to delete page
        [HttpGet]
        public ViewResult DeleteEmployee(Guid employeeId)
        {
            var employee = _context.Employees.Find(employeeId);
            return View(employee);
        }

        //actually removes employee from database then redirects to index
        [HttpPost, ActionName("DeleteEmployee")]
        public IActionResult ConfirmDeleteEmployee(Guid id)
        {
            var employeeToDelete = _context.Employees.Find(id);
            _context.Employees.Remove(employeeToDelete);
            _context.SaveChanges();
            return RedirectToAction("EmployeeIndex");
        }


        //finds employee and converts it to an employeecreatemodel for modifying.
        [HttpGet]
        public ViewResult EditEmployee(Guid employeeId)
        {
            Employee currentEmployee = _context.Employees.Find(employeeId);
            EmployeeCreateModel employeeCreateModel = new EmployeeCreateModel()
            {
                Id = currentEmployee.Id,
                Username = currentEmployee.Username,
                Password = currentEmployee.Password,
                Email = currentEmployee.Email,
                IsAdmin = currentEmployee.IsAdmin == true ? "Yes" : "No"
            };
            return View(employeeCreateModel);
        }

        [HttpPost]
        public IActionResult EditEmployee(EmployeeCreateModel employeechanges)
        {
            //find employee in database
            Employee employeeToBeUpdated = _context.Employees.Find(employeechanges.Id);
            //modify it here 
            employeeToBeUpdated.Username = employeechanges.Username;
            employeeToBeUpdated.Password = employeechanges.Password;    
            employeeToBeUpdated.Email= employeechanges.Email;
            employeeToBeUpdated.IsAdmin = employeechanges.IsAdmin == "Yes" ? true : false;

            _context.SaveChanges();

            return RedirectToAction("EmployeeIndex");
        }
    }
}
