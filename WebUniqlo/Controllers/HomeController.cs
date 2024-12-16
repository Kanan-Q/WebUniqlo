using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text.Json;
using WebUniqlo.DataAccess;
using WebUniqlo.ViewModel.Basket;
using WebUniqlo.ViewModel.Comment;
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
        public async Task<IActionResult> About()
        {
            return View();
        }
        public async Task<IActionResult> Contact()
        {
            return View();
        }
       

    }
}
