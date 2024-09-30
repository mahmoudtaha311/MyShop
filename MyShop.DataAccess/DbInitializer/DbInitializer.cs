using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Utilibilites;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task Initialize()
        {
            // Apply migrations if needed
            try
            {
                if ((await _context.Database.GetPendingMigrationsAsync()).Count() > 0)
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("Error applying migrations", ex);
            }

            // Check if roles exist and create them if they don't
            if (!await _roleManager.RoleExistsAsync(SD.AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.AdminRole));
                await _roleManager.CreateAsync(new IdentityRole(SD.EditorRole));
                await _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole));

                // Create Admin user
                var adminUser = new ApplicationUser
                {
                    UserName = "Admin123@myshop.com",
                    Email = "Admin123@myshop.com",
                    Name = "Administrator",
                    Address = "Damita",
                    City = "Zarqa",
                    PhoneNumber = "1234567890"
                };

                var result = await _userManager.CreateAsync(adminUser, "Mahmoud@!2001");

                // Check if the user was created successfully
                if (result.Succeeded)
                {
                    // Assign Admin role to the user
                    await _userManager.AddToRoleAsync(adminUser, SD.AdminRole);
                }
                else
                {
                    // Handle case where user creation failed (e.g., log the errors)
                    throw new Exception("Failed to create Admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
