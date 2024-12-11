using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Claims;
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
        public IActionResult ReadMore(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var data = _sql.Products.Where(x => x.Id == id && !x.IsDeleted).Include(x => x.Ratings).ThenInclude(x => x.User).FirstOrDefault();
            if (data is null) return NotFound();
            ViewBag.Rating = 5;
            if (User.Identity?.IsAuthenticated ?? false)
            {
                string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
                var rating = _sql.ProductRatings.Where(x => x.UserId == userId && x.ProductId == id).Select(x => x.Rating).FirstOrDefault();
                ViewBag.Rating = rating == 0 ? 5 : rating;
            }
            //var a=
            return View(data);
        }

        public IActionResult Rating(int productId, int rating)
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var data = _sql.ProductRatings.Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefault();
            if (data is null)
            {
                _sql.ProductRatings.Add(new WebUniqlo.Models.ProductRating
                {
                    ProductId = productId,
                    UserId = userId,
                    Rating = rating
                });
            }
            else
            {
                data.Rating= rating;
            }
            _sql.SaveChanges();
            return RedirectToAction(nameof(ReadMore), new
            {
                Id = productId,
            });

        }
    }
}
