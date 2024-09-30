using System.Linq.Expressions;

namespace MyShop.Entities.Services
{
    public interface IGenericRepository<T> where T : class
    {
        //_Context.Categories.Where( c => c.name == "Electronic.ToList();

        //_Context.Categories.include("Product).ToList();
        IEnumerable<T> GeTAll(Expression<Func<T,bool>>? Predicate = null, string? IncludeWord = null);
        T GetFirstOrDefualt(Expression<Func<T,bool>>? Predicate = null, string? IncludeWord = null);
        //_Context.Categories.Add(Category);

        void Add(T entity);
        //_Context.Categories.Remove(Category);

        void Delete(T entity);

        //_Context.Categories.RemoveRange(Category);

        void RemoveRange(IEnumerable<T> entities);

    }
}
