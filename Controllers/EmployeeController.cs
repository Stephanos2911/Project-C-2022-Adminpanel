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

        //Employee index page
        public IActionResult EmployeeIndex()
        {
            CheckLogin();
            return View(_context.Employees);
        }

        //Add Employee Page
        [HttpGet]
        public IActionResult AddEmployee() {
            CheckLogin();
            return View();
        }

        //Adds employee to database and redirects to index 
        [HttpPost]
        public IActionResult AddEmployee(EmployeeCreateModel newEmployeemodel)
        {
            CheckLogin();
            if (ModelState.IsValid)
            {
                Employee newEmployee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    Username = newEmployeemodel.Username,
                    Password = newEmployeemodel.Password,
                    Email = newEmployeemodel.Email,
                    IsAdmin = newEmployeemodel.IsAdmin == "Administrator" ? true : false
                };

                _context.Employees.Add(newEmployee);
                _context.SaveChanges();
            }

            return RedirectToAction("EmployeeIndex");
        }

        //Details page for employee
        public IActionResult EmployeeDetails(Guid id)
        {
            CheckLogin();
            var currentEmployee = _context.Employees.Find(id);
            if (currentEmployee == null)
            {
                return NotFound();
            }

            return View(currentEmployee);
        }

        //directs to delete page
        [HttpGet]
        public IActionResult DeleteEmployee(Guid id)
        {
            CheckLogin();
            return View(_context.Employees.Find(id));
        }

        //actually removes employee from database then redirects to index
        [HttpPost, ActionName("DeleteEmployee")]
        public IActionResult ConfirmDeleteEmployee(Guid id)
        {
            CheckLogin();
            _context.Employees.Remove(_context.Employees.Find(id));
            _context.SaveChanges();
            return RedirectToAction("EmployeeIndex");
        }


        //finds employee and converts it to an employeecreatemodel for modifying.
        [HttpGet]
        public IActionResult EditEmployee(Guid id)
        {
            CheckLogin();
            Employee currentEmployee = _context.Employees.Find(id);
            EmployeeCreateModel employeeCreateModel = new EmployeeCreateModel()
            {
                Id = currentEmployee.Id,
                Username = currentEmployee.Username,
                Password = currentEmployee.Password,
                Email = currentEmployee.Email,
                IsAdmin = currentEmployee.IsAdmin == true ? "Administrator" : "Medewerker"
            };
            return View(employeeCreateModel);
        }


        //updates the employee
        [HttpPost]
        public IActionResult EditEmployee(EmployeeCreateModel employeechanges)
        {
            CheckLogin();
            if (ModelState.IsValid)
            {
                //find employee in database
                Employee employeeToBeUpdated = _context.Employees.Find(employeechanges.Id);
                //modify it here 
                employeeToBeUpdated.Username = employeechanges.Username;
                employeeToBeUpdated.Password = employeechanges.Password;
                employeeToBeUpdated.Email = employeechanges.Email;
                employeeToBeUpdated.IsAdmin = employeechanges.IsAdmin == "Administrator" ? true : false;
                //saves changes made to record in database
                _context.SaveChanges();

                return RedirectToAction("EmployeeIndex");
            }
            return RedirectToAction("EmployeeIndex");
        }
    }
}
