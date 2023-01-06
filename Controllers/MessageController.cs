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

        //loads all messages into MessageIndex.html
        public IActionResult MessageIndex()
        {
            CheckLogin();

            //load list of all Messages into Index view
            return View(_context.Messages);
        }

        //sends message object to MessageDetails.html
        public IActionResult MessageDetails(Guid id)
        {
            CheckLogin();
            return View(_context.Messages.Find(id));
        }

        //sets the IsAnswered property of the message to true, then saves it to database.
        public IActionResult AnswerMessage(Guid id)
        {
            CheckLogin();
            var Message = _context.Messages.Find(id);
            Message.IsAnswered = true;
            Message.EmployeeId = CurrentEmployee.currentEmployee.Id;
            _context.SaveChanges();
            return RedirectToAction("MessageDetails", new { id = id });
        }


        [HttpGet]
        public IActionResult DeleteMessage(Guid id) 
        {
            CheckLogin();
            return View(id);
        }

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
