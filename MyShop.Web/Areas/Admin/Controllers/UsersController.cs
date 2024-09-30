using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Data;
using MyShop.Utilibilites;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminRole)]

    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;

        public UsersController(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
        }
        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;


            return View(context.AplicationUsers.Where(u => u.Id != userid));
        }
        public IActionResult LockUnlock(string ? id) 
        {
            var user = context.AplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null) 
            {
                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else 
            {
                user.LockoutEnd = DateTime.Now;
            }

           context.SaveChanges();
            return RedirectToAction("Index" , "users", new {area = "Admin"});
        } 
    }
}
