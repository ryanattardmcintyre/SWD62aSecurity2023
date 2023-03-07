using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Validators;

namespace WebApplication1.Controllers
{
    public class ReservationsController : Controller
    {

        private BooksRepository _br;
        private UserManager<IdentityUser> _userManager;
        public ReservationsController(BooksRepository br, UserManager<IdentityUser> userManager) { 
            
            _br = br;
            _userManager = userManager;
        
        } 


        [HttpGet]
        public IActionResult Reserve()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        [ServiceFilter(typeof(PermissionsActionFilter))]
        public IActionResult Reserve(ReservationViewModel r)
        {
            if (ModelState.IsValid)
            {
                //string id = _userManager.GetUserId(User);
                string id = User.Claims.ElementAt(0).Value;

                if (id == null) return RedirectToAction("Reserve");

                Reservation res = new Reservation()
                {
                    Isbn_Fk = r.Isbn,
                    From
                      = r.From,
                    To = r.To
                     ,
                    User_Fk = id
                };

                try
                {
                    _br.Reserve(res);
                }
                catch (Exception ex)
                {
                    //log
                    TempData["error"] = "Error reserving the book. contact admin";
                }
            }
            return View();
        }
    }
}
