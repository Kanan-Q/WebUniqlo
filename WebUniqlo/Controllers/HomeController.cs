using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebUniqlo.DataAccess;
using WebUniqlo.Models;
using WebUniqlo.ViewModel.Common;
using WebUniqlo.ViewModel.Products;
using WebUniqlo.ViewModel.Sliders;

namespace Uniqlo.Controllers
{
    public class HomeController(UniqloDbContext _sql) : Controller
    {
        public async Task<IActionResult> Index()
        {
            HomeVM hm = new();
            hm.Sliders = await _sql.Sliders
            .Where(x => !x.IsDeleted)
                 .Select(x => new SliderItem
                 {
                     ImageUrl = x.ImageUrl,
                     Link = x.Link,
                     Title = x.Title,
                     Subtitle = x.Subtitle
                 }).ToListAsync();

            hm.Products = await _sql.Products.Where(x => !x.IsDeleted).Select(x => new ProductItemVM
            {
                Discount = x.Discount,
                Id = x.Id,
                IsInStock = x.Quantity > 0,
                Name = x.Name,
                Price = x.SellPrice,
                ImageURL = x.CoverFile,
            }).ToListAsync();

            return View(hm);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult ReadMore(int id)
        {
            var a = _sql.Products.FirstOrDefault(x => x.Id == id);
            return View(a);
        }
    }
}
