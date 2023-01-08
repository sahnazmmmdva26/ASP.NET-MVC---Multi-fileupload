using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProniaSite.DAL;
using ProniaSite.Models;
using ProniaSite.Utilies;
using ProniaSite.ViewModels;

namespace ProniaSite.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }


        //public ProductController(IWebHostEnvironment env)
        //{
        //    _env = env;
        //}

        public IActionResult Index()
        {
            return View(_context.Products.Include(p=>p.ProductColors).ThenInclude(pc=>pc.Color).Include(p=>p.ProductSizes).ThenInclude(ps=>ps.Size).Include(p=>p.ProductImages));
        }
        public IActionResult Create()
        {
            
            ViewBag.Colors = new SelectList(_context.Colors,"Id","Name");
            ViewBag.Sizes = new SelectList(_context.Sizes,"Id","Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateProductVM cp)
        {
            var coverimg = cp.CoverImage;
            var hoverimg =cp.HoverImage;
            var otherimg = cp.OtherImages;
            
            if(hoverimg?.CheckType("image/")==false)
            {
                ModelState.AddModelError("Hover", "yuklediyiniz fayl shekil deyil");
            }
            if (hoverimg?.CheckSize(300) == false)
            {
                ModelState.AddModelError("Hover", "yuklediyiniz faylin olcusu 300kb dan az olmalidi");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Colors = new SelectList(_context.Colors, "Id", "Name");
                ViewBag.Sizes = new SelectList(_context.Sizes, "Id", "Name");
                return View();
            }
            var sizes = _context.Sizes.Where(s => cp.SizeIds.Contains(s.Id));
            var colors = _context.Colors.Where(c => cp.ColorIds.Contains(c.Id));
          
            Product newProduct = new Product
            {
                Name = cp.Name,
                CostPrice = cp.CostPrice,
                SellPrice = cp.SellPrice,
                Description = cp.Description,
                Discount = cp.Discount,
                IsDeleted = false,
                SKU = "1"
            };
            List<ProductImage> images = new List<ProductImage>();
            images.Add(new ProductImage
            {
                ImageUrl = coverimg.SaveFile(Path.Combine(_env.WebRootPath, "assets",
                "images", "product")),
                IsCover = true,
                Product = newProduct
            });
            foreach (var item in otherimg)
            {
                if (item?.CheckType("image/") == false)
                {
                    ModelState.AddModelError("Hover", "yuklediyiniz fayl shekil deyil");
                }
                if (item?.CheckSize(300) == false)
                {
                    ModelState.AddModelError("Hover", "yuklediyiniz faylin olcusu 300kb dan az olmalidi");
                }
                images.Add(new ProductImage
                {
                    ImageUrl = coverimg.SaveFile(Path.Combine(_env.WebRootPath, "assets",
               "images", "product")),
                    IsCover = null,
                    Product = newProduct
                });
            }

            newProduct.ProductImages = images;
            _context.Products.Add(newProduct);
            foreach (var item in colors)
            {
                _context.ProductColors.Add(new ProductColor { Product=newProduct,ColorId=item.Id});
            }
            foreach (var item in sizes)
            {
                _context.ProductSizes.Add(new ProductSize { Product = newProduct, SizeId = item.Id });
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}

  

   