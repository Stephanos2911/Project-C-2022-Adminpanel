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

        public RedirectToActionResult CheckLogin()
        {
            if (!CurrentEmployee.IsLoggedIn())
            {
                return RedirectToAction("LoginPage", "Access");
            }
            return null;
        }

        //loads all messages in order of unanswered first into MessageIndex.html
        public IActionResult MessageIndex()
        {
            CheckLogin();
            var allMessages = _context.Messages;
            return View(allMessages.OrderBy(x => x.IsAnswered));
        }

        //sends message object to MessageDetails.html
        public IActionResult MessageDetails(Guid id)
        {
            CheckLogin();
            return View(_context.Messages.Find(id));
        }

       
        //sets the IsAnswered property of the message to true and adds the employee name who answered it, then saves it to database.
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


        [HttpGet]
        public IActionResult DeleteMessage(Guid id) 
        {
            CheckLogin();
            return View(id);
        }

        //deletes message from database
        [HttpPost, ActionName("DeleteMessage")]
        public IActionResult ConfirmDeleteMessage(Guid id)
        {
            CheckLogin();
            //Delete message from db
            _context.Messages.Remove(_context.Messages.Find(id));
            _context.SaveChanges();

            //redirect to index
            return RedirectToAction("MessageIndex");
        }
    }
}
