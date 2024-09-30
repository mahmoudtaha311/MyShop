using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Data;
using MyShop.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace MyShop.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            // context.Table    T => Type table 
            dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            //Categories.Add(Category);
            dbSet.Add(entity);

        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public IEnumerable<T> GeTAll(Expression<Func<T, bool>>? Predicate = null, string? IncludeWord = null)
        {
            IQueryable<T> query = dbSet;
            if (Predicate != null)
            {
                query = query.Where(Predicate);
            }
            if (IncludeWord != null)
            {
                //context.Products.include("categories")
                foreach (var  item in IncludeWord.Split(new char[] {','} ,StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }

            }
            return query.ToList();
        }

        public T GetFirstOrDefualt(Expression<Func<T, bool>>? Predicate = null, string? IncludeWord = null)
        {
            IQueryable<T> query = dbSet;
            if (Predicate != null)
            {
                query = query.Where(Predicate);
            }
            if (IncludeWord != null)
            {
                //context.Products.include("categories")
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }

            }

            return query.SingleOrDefault();
        }

        public void RemoveRange(IEnumerable<T> entities)
            {
                dbSet.RemoveRange(entities);
            }
        }

       
    } 

