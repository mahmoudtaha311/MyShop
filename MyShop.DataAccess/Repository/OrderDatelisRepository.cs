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
    internal class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext context;

        public OrderDetailsRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
     
        }

        public void Update(OrderDetails orderDetails)
        {
            context.Update(orderDetails);
            context.SaveChanges();
        }
    }
}
