using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebUniqlo.DataAccess;
using WebUniqlo.Models;
using WebUniqlo.Extension;
using WebUniqlo.Helper;
using WebUniqlo.Services.Abstrations;
using WebUniqlo.Services.Interfaces;

namespace WebUniqlo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<UniqloDbContext>(x =>
            {
                x.UseSqlServer(builder.Configuration.GetConnectionString("MSSql"));
            });
            builder.Services.AddIdentity<User, IdentityRole>(x =>
            {
                //x.User.AllowedUserNameCharacters = "assa1234d";
                x.SignIn.RequireConfirmedEmail = true;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireDigit = true;
                x.Password.RequireLowercase = true;
                x.Password.RequireUppercase = true;
                x.Lockout.MaxFailedAccessAttempts = 20;
                x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDbContext>();

            builder.Services.AddScoped<IEmailService, EmailService>();
            var opt = new smtpOptions();
            builder.Services.Configure<smtpOptions>(builder.Configuration.GetSection("smtp"));
            builder.Services.Configure<smtpOptions>(builder.Configuration.GetSection(smtpOptions.Name));

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseUserSeed();
            app.UseAuthorization();
            app.UseRouting();

            app.UseAuthorization();

            //app.UseUserSeed(); 
            app.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                    );
            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
