using MyShop.Entities.Models;

namespace MyShop.Entities.Services
{
    public interface IcategoryRepository : IGenericRepository<Category>
    {
        void Update(Category category);
    }
}
