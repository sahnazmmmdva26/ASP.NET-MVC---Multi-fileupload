using Microsoft.EntityFrameworkCore;
using ProniaSite.DAL;

namespace ProniaSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer("Server=DESKTOP-59QHG10;Database=Pronia;Trusted_Connection=True;Integrated Security=true;Encrypt=false;");
            });
            var app = builder.Build();

            app.UseRouting();
            app.UseStaticFiles();
            app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}