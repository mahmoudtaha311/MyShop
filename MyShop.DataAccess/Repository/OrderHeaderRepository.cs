using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Repository
{
    internal class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext context;

        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            context.OrderHeaders.Update(orderHeader);
            context.SaveChanges();
        }

        public void UpdateOrderStatus(int id, string OrderStatus, string? PaymentStatus)
        {
            var orderfdb = context.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if (orderfdb != null) 
            {
                orderfdb.OrderStatus = OrderStatus;
                orderfdb.PaymentDate = DateTime.Now;

                if (PaymentStatus != null) 
                {
                    orderfdb.PaymentStatus = PaymentStatus;
                }
            }

            context.SaveChanges();

        }
    }
}
