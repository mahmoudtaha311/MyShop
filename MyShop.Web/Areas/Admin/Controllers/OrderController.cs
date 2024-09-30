using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.Entities.Services;
using MyShop.Entities.ViewModels;
using MyShop.Utilibilites;
using Stripe;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminRole)]

    [Area("Admin")]

    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public IActionResult GetData() 
        {
            IEnumerable<OrderHeader> orderHeaders = unitOfWork.OrderHeader.GeTAll(IncludeWord: "applicationuser");

            return Json(new {data = orderHeaders});
        }
        [HttpGet]
        public IActionResult Details(int orderid) 
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = unitOfWork.OrderHeader.GetFirstOrDefualt(o => o.Id == orderid , IncludeWord: "applicationuser"),
                OrderDetails = unitOfWork.OrderDetails.GeTAll(IncludeWord: "Product")
            };
            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult updateorderDetails( ) 
        {
            var orderfdb = unitOfWork.OrderHeader.GetFirstOrDefualt(o => o.Id == OrderVM.OrderHeader.Id);
            orderfdb.Name = OrderVM.OrderHeader.Name;
            orderfdb.Address = OrderVM.OrderHeader.Address;
            orderfdb.City = OrderVM.OrderHeader.City;
            orderfdb.Phone = OrderVM.OrderHeader.Phone;
            if (OrderVM.OrderHeader.Carrier != null)
            {
                orderfdb.Carrier = OrderVM.OrderHeader.Carrier;

            }
            if (orderfdb.TrackingNumber != null) 
            {
                orderfdb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
            unitOfWork.OrderHeader.Update(orderfdb);
            unitOfWork.Complete();
            TempData["Update"] = " Data Has Updated Successfully ";

            return RedirectToAction("details", "order", new { orderid = orderfdb.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult StartProccess()
		{
            unitOfWork.OrderHeader.UpdateOrderStatus(OrderVM.OrderHeader.Id, SD.Processing, null);
            unitOfWork.Complete();
			
			TempData["Update"] = " Order Status Has Updated Successfully ";

			return RedirectToAction("details", "order", new { orderid = OrderVM.OrderHeader.Id });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip()
		{
			var orderfdb = unitOfWork.OrderHeader.GetFirstOrDefualt(o => o.Id == OrderVM.OrderHeader.Id);
            orderfdb.TrackingNumber =OrderVM.OrderHeader.TrackingNumber;
            orderfdb.Carrier =OrderVM.OrderHeader.Carrier;
            orderfdb.OrderStatus =SD.Shipped;
            orderfdb.ShippingDate = DateTime.Now;
            unitOfWork.OrderHeader.Update(orderfdb);

			unitOfWork.Complete();

			TempData["Update"] = " Order  Has Shipped Successfully ";

			return RedirectToAction("details", "order", new { orderid = OrderVM.OrderHeader.Id });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
		{
			var orderfdb = unitOfWork.OrderHeader.GetFirstOrDefualt(o => o.Id == OrderVM.OrderHeader.Id);
            if(orderfdb.PaymentStatus == SD.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderfdb.PaymentIntentId,

                };
                var service = new RefundService();
                Refund refund = service.Create(option);
				unitOfWork.OrderHeader.UpdateOrderStatus(orderfdb.Id, SD.Cancelled, SD.Refund);

			}
            else
            {

				unitOfWork.OrderHeader.UpdateOrderStatus(orderfdb.Id, SD.Cancelled, SD.Cancelled);

			}

			unitOfWork.Complete();

			TempData["Update"] = " Order  Has Cancelled Successfully ";

			return RedirectToAction("details", "order", new { orderid = orderfdb.Id });
		}
	}
}
