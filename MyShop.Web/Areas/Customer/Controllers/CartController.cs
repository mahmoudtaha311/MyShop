using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.Entities.Services;
using MyShop.Entities.ViewModels;
using MyShop.Utilibilites;
using Stripe.Checkout;
using Stripe;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM  CartVM { get; set; }
        public int TotalCarts { get; set; }
        
        public CartController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;
            CartVM = new ShoppingCartVM()
            {
                CartList = _unitOfWork.ShoppingCart.GeTAll(c => c.applicationuserId == userid, IncludeWord:"Product")
            };
            foreach (var item in CartVM.CartList)
            {
                CartVM.TotalCarts += item.Count * item.Product.Price;
            }
            
            return View(CartVM);
        }
        [HttpGet]
        public ActionResult Summary() 
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;
            CartVM = new ShoppingCartVM()
            {
                CartList = _unitOfWork.ShoppingCart.GeTAll(c => c.applicationuserId == userid, IncludeWord: "Product"),
                OrderHeader = new()


            };
            CartVM.OrderHeader.applicationuser = _unitOfWork.ApplicationUser.GetFirstOrDefualt(u => u.Id == userid);
            CartVM.OrderHeader.Name = CartVM.OrderHeader.applicationuser.Name;
            CartVM.OrderHeader.Address = CartVM.OrderHeader.applicationuser.Address;
            CartVM.OrderHeader.City = CartVM.OrderHeader.applicationuser.City;
            CartVM.OrderHeader.Phone = CartVM.OrderHeader.applicationuser.PhoneNumber;

            foreach (var item in CartVM.CartList)
            {
                CartVM.OrderHeader.TotalPrice += item.Count * item.Product.Price;
            }
            ViewData["count"] = _unitOfWork.ShoppingCart.GeTAll(d => d.applicationuserId == userid).Count();
            return View(CartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public ActionResult PostSummary(ShoppingCartVM shoppingCartVM)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;

            shoppingCartVM.CartList = _unitOfWork.ShoppingCart.GeTAll(c => c.applicationuserId == userid, IncludeWord: "Product");

            shoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
            shoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;
            shoppingCartVM.OrderHeader.OrderDate =DateTime.Now;
            shoppingCartVM.OrderHeader.applicationuserid = userid;
          

            foreach (var item in shoppingCartVM.CartList)
            {
                shoppingCartVM.TotalCarts += item.Count * item.Product.Price;
            }
            shoppingCartVM.OrderHeader.TotalPrice = shoppingCartVM.TotalCarts;
          
            _unitOfWork.OrderHeader.Add(shoppingCartVM.OrderHeader);
            _unitOfWork.Complete();

            foreach (var item in shoppingCartVM.CartList)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderID = shoppingCartVM.OrderHeader.Id,
                    TotalPrice = shoppingCartVM.TotalCarts,
                    Count = item.Count
                };
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Complete();
            }

                var domain = "https://localhost:44390/";
                
                     var options = new SessionCreateOptions
                     {

                         LineItems = new List<SessionLineItemOptions>(),

                         Mode = "payment",
                         SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                         CancelUrl = domain + $"Customer/Cart/Index",

                     };
                foreach (var item in shoppingCartVM.CartList)
                {

                    var SessionLineOptions = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Product.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,
                            },
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(SessionLineOptions);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                shoppingCartVM.OrderHeader.SessionId = session.Id;

                _unitOfWork.Complete();
                ViewData["count"] = _unitOfWork.ShoppingCart.GeTAll(d => d.applicationuserId == userid).Count();


                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
                //_unitOfWork.ShoppingCart.RemoveRange(shoppinCartVM.CartList);
                //_unitOfWork.Complete();
                //return RedirectToAction("Index", "Home");
            }

           
            
        
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeaders = _unitOfWork.OrderHeader.GetFirstOrDefualt(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeaders.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateOrderStatus(id, SD.Approve, SD.Approve);
                orderHeaders.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.Complete();
            }
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GeTAll(u => u.applicationuserId == orderHeaders.applicationuserid).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Complete();
            return View(id);
        }

        public IActionResult Plus(int id) 
        {
        var shoppingcart = _unitOfWork.ShoppingCart.GetFirstOrDefualt(s => s.ShopingCardId == id);
            _unitOfWork.ShoppingCart.IncreaseShoppingCart(shoppingcart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("index");
       
        
        }
        public IActionResult Minus(int id)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetFirstOrDefualt(s => s.ShopingCardId == id);
            if(shoppingcart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Delete(shoppingcart);
               
                var count = _unitOfWork.ShoppingCart.GeTAll(u => u.applicationuserId == shoppingcart.applicationuserId).ToList().Count() - 1;
                HttpContext.Session.SetInt32(SD.SessionKey, count);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecresaeShoppingCart(shoppingcart, 1);
              

            }
            _unitOfWork.Complete();
            return RedirectToAction("index");


        }
        public IActionResult Remove(int id)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetFirstOrDefualt(s => s.ShopingCardId == id);
            _unitOfWork.ShoppingCart.Delete(shoppingcart);
            _unitOfWork.Complete();
            var count = _unitOfWork.ShoppingCart.GeTAll(u => u.applicationuserId == shoppingcart.applicationuserId).ToList().Count();
            HttpContext.Session.SetInt32(SD.SessionKey, count);
            return RedirectToAction("index");


        }
    }

}
