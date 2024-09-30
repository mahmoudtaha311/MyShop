using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Repository;
using MyShop.Entities.Services;
using MyShop.Utilibilites;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyShop.Web.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            // تحقق من أن المستخدم مسجل الدخول
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var userId = claim.Value;
                HttpContext.Session.Remove(SD.SessionKey);

                if (HttpContext.Session.GetInt32(SD.SessionKey) != null)
                {
                    // إرجاع العدد المخزن في الجلسة
                    return View(HttpContext.Session.GetInt32(SD.SessionKey));
                }
                else
                {
                    // جلب عدد العناصر من عربة التسوق للمستخدم وتخزينها في الجلسة
                    int cartItemCount = unitOfWork.ShoppingCart.GeTAll(u => u.applicationuserId == userId).ToList().Count();
                    HttpContext.Session.SetInt32(SD.SessionKey, cartItemCount);
                    return View(cartItemCount);
                }
            }
            else
            {
                // إذا لم يكن المستخدم مسجل الدخول، قم بمسح الجلسة وعرض 0
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
