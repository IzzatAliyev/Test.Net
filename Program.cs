using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseUrls("https://localhost:7046");

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DirectoryContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Directory/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Directory}/{action=Main}/{id?}");

            app.Run();
        }
    }
}
