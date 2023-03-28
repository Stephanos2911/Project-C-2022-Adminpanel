using AdminApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Project_C.Models.UserModels;

namespace Project_C.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This function checks if the user is logged in, and redirects them to the login page if they are not
        public RedirectToActionResult CheckLogin()
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                return RedirectToAction("LoginPage", "Access");
            }
            return null;
        }

        // This function loads all messages and orders them by unanswered first, and displays them in the MessageIndex view
        public IActionResult MessageIndex()
        {
            CheckLogin();
            var allMessages = _context.Messages;
            return View(allMessages.OrderBy(x => x.IsAnswered));
        }

        // This function displays a specific message by ID in the MessageDetails view
        public IActionResult MessageDetails(Guid id)
        {
            CheckLogin();
            return View(_context.Messages.Find(id));
        }

        // This function sets the IsAnswered property of a message to true, adds the name of the employee who answered it, and saves it to the database
        public IActionResult AnswerMessage(Guid id)
        {
            CheckLogin();
            var Message = _context.Messages.Find(id);
            Message.IsAnswered = true;
            Message.EmployeeId = CurrentEmployee.currentEmployee.Id;
            Message.EmployeeName = CurrentEmployee.currentEmployee.Username;
            _context.SaveChanges();
            return RedirectToAction("MessageDetails", new { id = id });
        }

        // This function displays the DeleteMessage view for a specific message by ID
        [HttpGet]
        public IActionResult DeleteMessage(Guid id)
        {
            CheckLogin();
            return View(id);
        }

        // This function deletes a specific message by ID from the database
        [HttpPost, ActionName("DeleteMessage")]
        public IActionResult ConfirmDeleteMessage(Guid id)
        {
            CheckLogin();
            // Delete the message from the database
            _context.Messages.Remove(_context.Messages.Find(id));
            _context.SaveChanges();

            // Redirect to the MessageIndex page
            return RedirectToAction("MessageIndex");
        }

    }
}
