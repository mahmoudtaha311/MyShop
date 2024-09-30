
using MyShop.DataAccess.Repository;
using MyShop.DataAccess.Data;

using MyShop.Entities.Models;
using MyShop.Entities.Services;
using Microsoft.EntityFrameworkCore;



namespace MyShop.DataAccess.Repository;



public class ProductRepository :GenericRepository<Product> ,IProductRepository
{
     private readonly ApplicationDbContext context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }





    public void Update(Product product)
    {
        var Productold = context.Products.AsTracking().FirstOrDefault(c => c.Id == product.Id);
        if (Productold != null)
        {
            Productold.Name = product.Name;
            Productold.Description = product.Description;
            Productold.Price = product.Price;
            Productold.Img = product.Img;
            Productold.categoryId = product.categoryId;
            
            
        }
       
    }


   
}
