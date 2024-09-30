
using MyShop.DataAccess.Repository;
using MyShop.DataAccess.Data;

using MyShop.Entities.Models;
using MyShop.Entities.Services;
using Microsoft.EntityFrameworkCore;



namespace MyShop.DataAccess.Repository;



public class ShoppingcardRepository :GenericRepository<ShoppingCart> ,IShoppingCart
{
     private readonly ApplicationDbContext context;

    public ShoppingcardRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }

    public int DecresaeShoppingCart(ShoppingCart shoppingCart, int count)
    {
       shoppingCart.Count -= count;
        return shoppingCart.Count;
    }

    public int IncreaseShoppingCart(ShoppingCart shoppingCart, int count)
    {
        shoppingCart.Count += count;
        return shoppingCart.Count;
    }
}
