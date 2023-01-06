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

        public IActionResult DirectToLogin()
        {
            return RedirectToAction("LoginPage", "Access");
        }


        public IActionResult MessageIndex()
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                return DirectToLogin();
            }

            //load list of all Messages into Index view
            return View(_context.Messages);
        }

        public IActionResult MessageDetails(Guid id)
        {
            return View(_context.Messages.Find(id));
        }

        //sets the IsAnswered property of the message to true.
        public IActionResult AnswerMessage(Guid id)
        {
            var Message = _context.Messages.Find(id);
            Message.IsAnswered = true;
            Message.EmployeeId = CurrentEmployee.currentEmployee.Id;
            _context.SaveChanges();
            return RedirectToAction("MessageDetails", new { id = id });
        }


        [HttpGet]
        public IActionResult DeleteMessage(Guid id) 
        {
            return View(id);
        }

        [HttpPost, ActionName("DeleteMessage")]
        public IActionResult ConfirmDeleteMessage(Guid id)
        {
            //Delete message from db
            _context.Messages.Remove(_context.Messages.Find(id));
            _context.SaveChanges();

            //redirect to index
            return RedirectToAction("MessageIndex");
        }
    }
}
