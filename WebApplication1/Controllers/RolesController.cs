using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        RolesRepository _rr;
        public RolesController(RolesRepository rr) { _rr = rr; }

        //to do block access to the index page if user is not an admin

     
        public IActionResult Index()
        {
            return View();
        }
    
        public IActionResult Allocate(string email, string role) {
        
            _rr.AllocateRole(email, role);
            TempData["message"] = "Role has been allocate to user " + email;
            return RedirectToAction("Index");
        }
    }
}
