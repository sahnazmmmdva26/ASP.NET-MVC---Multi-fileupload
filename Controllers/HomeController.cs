using Microsoft.AspNetCore.Mvc;
using ProniaSite.DAL;
using ProniaSite.Models;
using ProniaSite.ViewModels;

namespace ProniaSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM home = new HomeVM
            {
                IndexMainSlides = _context.IndexMainSlides,
                Shippings=_context.Shippings,
                Banners=_context.Banners,
                Brands=_context.Brands,
                ClientSlides=_context.ClientSlides,
                Products= _context.Products,
            };
            return View(home);
        }
        public IActionResult SingleProduct()
        {
            return View();
        }
        public IActionResult Shop()
        {
            List<Category> Categories = new List<Category>();
           
            
                Categories = _context.Categories.ToList();
            
            List<Color> Colors = new List<Color>();
            
                Colors = _context.Colors.ToList();
            
            ViewBag.Colors=Colors;
            return View(Categories);
        }
        public IActionResult Card()
        {
            return View();
        }
        public IActionResult LoginRegister()
        {
            return View();
        }
         
    }
}
