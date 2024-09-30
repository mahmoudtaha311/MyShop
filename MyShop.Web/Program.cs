using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using MyShop.DataAccess.Data;
using MyShop.DataAccess.DbInitializer;
using MyShop.DataAccess.Repository;
using MyShop.Entities.Services;
using MyShop.Utilibilites;
using NuGet.Protocol.Core.Types;
using Stripe;
using System.Text.Json.Serialization;

namespace MyShop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

			builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IDbInitializer,DbInitializer >();

            builder.Services.AddSingleton<IEmailSender,EmailSender>();
      

            builder.Services.AddDbContext<ApplicationDbContext>(OptionsBuilder =>
            {
                OptionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection"));
            });
            builder.Services.Configure<StripeData>(builder.Configuration.GetSection("Strip"));
            builder.Services.AddIdentity<IdentityUser,IdentityRole>(Options => Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5))
                .AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Strip:Secretkey").Get<string>();

            SeedDb();
            app.MapRazorPages();
           
            app.UseAuthorization();
            app.UseSession();



            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}",
                defaults: new { controller = "Home", action = "Index" }
                                        );


            app.Run();

            void SeedDb()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbinitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    dbinitializer.Initialize();
                }
        }

        }
       
    }
}
