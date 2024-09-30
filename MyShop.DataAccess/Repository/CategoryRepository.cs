
using MyShop.DataAccess.Repository;
using MyShop.DataAccess.Data;

using MyShop.Entities.Models;
using MyShop.Entities.Services;
using Microsoft.EntityFrameworkCore;



namespace MyShop.DataAccess.Repository;



public class CategoryRepository :GenericRepository<Category> ,IcategoryRepository
{
     private readonly ApplicationDbContext context;

    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }





    public void Update(Category category)
    {
        var categoryold = context.Categories.AsTracking().FirstOrDefault(c => c.Id == category.Id);
        if (categoryold != null)
        {
            categoryold.Name = category.Name;
            categoryold.Description = category.Description;
            categoryold.CreatedDate = DateTime.Now;
            
        }
       
    }


    //public Category GetCategory(int id)
    //{

    //    var category = context.Categories.FirstOrDefault(c => c.Id == id);

    //    if (category != null)
    //    {
    //        return category;
    //    }
    //    return null;
    //}
    //public List<Category> GetAllCategories()
    //{
    //    var categories = context.Categories.ToList();

    //    if (categories != null)
    //    {
    //        return categories;
    //    }
    //    return null;
    //}
    //public void AddCategory(Category category)
    //{

    //    context.Categories.Add(category);
    //    context.SaveChanges();


    //}
    //public void EditCategory(int id, Category category)
    //{

    //    var categoryold = GetCategory(id);
    //    categoryold.Name = category.Name;
    //    categoryold.Description = category.Description;

    //    context.SaveChanges();

    //}

    //public void DeleteCategory(int id)
    //{
    //    var category = GetCategory(id);
    //    context.Categories.Remove(category);
    //    context.SaveChanges();

    //}
}
