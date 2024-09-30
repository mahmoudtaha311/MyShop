using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Services
{
    public interface IShoppingCart :IGenericRepository<ShoppingCart>
    {
        int IncreaseShoppingCart(ShoppingCart shoppingCart, int count);
        int DecresaeShoppingCart(ShoppingCart shoppingCart, int count);

    }
}
