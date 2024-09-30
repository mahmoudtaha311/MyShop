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
    public class ApplicationUserRepository:GenericRepository<ApplicationUser>,IApplicationUserRepository
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
