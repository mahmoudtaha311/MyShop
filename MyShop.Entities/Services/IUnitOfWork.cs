namespace MyShop.Entities.Services
{
    public interface IUnitOfWork:IDisposable
    {
        IcategoryRepository Category { get; }
        IProductRepository Product { get; }
        IShoppingCart ShoppingCart { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IApplicationUserRepository ApplicationUser { get; }
        int Complete();

    }
}
