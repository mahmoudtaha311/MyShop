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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        public IcategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }
        public IShoppingCart ShoppingCart { get; private set; }
        public IOrderDetailsRepository OrderDetails  { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext context )
        {
            this.context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            ShoppingCart = new ShoppingcardRepository(context);
            OrderHeader = new OrderHeaderRepository(context);
            OrderDetails = new OrderDetailsRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);

        }

        public int Complete()
        {
            return   context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
