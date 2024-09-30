using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Services;
using MyShop.Utilibilites;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class DashBoard : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DashBoard(IUnitOfWork unitOfWork) 
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        { 
            ViewBag.Orders = unitOfWork.OrderHeader.GeTAll().Count();
            ViewBag.ApprovedOrders = unitOfWork.OrderHeader.GeTAll().Count( x => x.OrderStatus == SD.Approve);
            ViewBag.Users = unitOfWork.ApplicationUser.GeTAll().Count();
            ViewBag.Products = unitOfWork.Product.GeTAll().Count();
            return View();
        }
    }
}
