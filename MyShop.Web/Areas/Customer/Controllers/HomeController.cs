using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.Entities.Services;
using MyShop.Entities.ViewModels;
using MyShop.Utilibilites;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork ;

        public HomeController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ;
        }
        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            int PageSize = 8;


            var product = _unitOfWork.Product.GeTAll().ToPagedList(pageNumber, PageSize);
            return View(product);
        }
       // [Route("{productId}")]
        public IActionResult Details(int id)
        {
        //    var prodid = productId;
                
            var product = _unitOfWork.Product.GetFirstOrDefualt(x => x.Id == id, IncludeWord: "Category");
          var   proid  = product.Id;
            ShoppingCart obj = new ShoppingCart()
            {
               ProductId = proid,

                Product = product,
                Count = 1
            };
    
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart? shoppingCart)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.applicationuserId = claim.Value;

            ShoppingCart shopcartoj = _unitOfWork.ShoppingCart.GetFirstOrDefualt(u => u.applicationuserId == claim.Value && u.ProductId == shoppingCart.ProductId);

            if(shopcartoj == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Complete();

                HttpContext.Session.SetInt32(SD.SessionKey,
                    _unitOfWork.ShoppingCart.GeTAll(u =>u.applicationuserId == shoppingCart.applicationuserId).ToList().Count());
            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseShoppingCart(shopcartoj, shoppingCart.Count);
				_unitOfWork.Complete();


			}
            return RedirectToAction("index");
        }

    }
}
