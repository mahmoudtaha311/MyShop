using MyShop.Entities.Models;

namespace MyShop.Entities.Services
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
