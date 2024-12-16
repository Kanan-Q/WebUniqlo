using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;
using System.Text.Json;
using WebUniqlo.DataAccess;
using WebUniqlo.Models;
using WebUniqlo.ViewModel.Basket;
using WebUniqlo.ViewModel.Comment;
using WebUniqlo.ViewModel.Products;
using static WebUniqlo.ViewModel.Products.ProductIndexVM;

namespace WebUniqlo.Controllers
{
    public class ProductsController(UniqloDbContext _sql) : Controller
    {
        public async Task<IActionResult> Shop()
        {
            IQueryable<Product> query = _sql.Products.Where(x => !x.IsDeleted);
            ProductIndexVM pm = new()
            {
                Products = await query.Select(x => new ProductItemVM
                {

                    Discount = x.Discount,
                    Id = x.Id,
                    IsInStock = x.Quantity > 0,
                    Name = x.Name,
                    Price = x.SellPrice,
                    ImageURL = x.CoverFile,
                }).ToListAsync(),
                Categories = [new CategoryAndCount
                {
                    Id = 0,
                    Count= await query.CountAsync(),
                    Name="All"
                }]
            };

            pm.Categories.AddRange(await _sql.Categories.Where(x => !x.IsDeleted).Select(x => new CategoryAndCount
            {
                Name = x.Name,
                Id = x.Id,
                Count = x.Products.Count()

            }).ToListAsync());


            return View(pm);
        }

        public async Task<IActionResult> Filter(int? catid, string? price = null)
        {
            if (!catid.HasValue) return BadRequest();
            var query = _sql.Products.Where(x => !x.IsDeleted);
            if (catid != 0)
            {
                query = query.Where(x => x.CategoryId == catid);
            }
            var data = await query.Select(x => new ProductItemVM
            {

                Discount = x.Discount,
                Id = x.Id,
                IsInStock = x.Quantity > 0,
                Name = x.Name,
                Price = x.SellPrice,
                ImageURL = x.CoverFile,
            }).ToListAsync();

            return PartialView("_ShopPartial",data);
        }

        public async Task<IActionResult> ReadMore(int? id)
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
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Rating(int productId, int rating)
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
                data.Rating = rating;
            }
            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(ReadMore), new
            {
                Id = productId,
            });

        }

        [Authorize]
        public async Task<IActionResult> AddComment(int productId, CommentCreateVM cm)
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var data = _sql.Comments.Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefault();
            if (data is null)
            {
                _sql.Comments.Add(new WebUniqlo.Models.Comment
                {
                    ProductId = productId,
                    UserId = userId,
                    Description = cm.Description,
                });
            }
            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(ReadMore), new
            {
                Id = productId,
            });
        }
        public async Task<IActionResult> RemoveComment(int? id)
        {
            if (!id.HasValue) return BadRequest();
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            var data = await _sql.Comments.FindAsync(id);

            ViewBag.UserId = userId;

            if (data is null) return NotFound();

            _sql.Comments.Remove(data);

            await _sql.SaveChangesAsync();

            return RedirectToAction(nameof(ReadMore), new { Id = data.ProductId });
        }

        public async Task<IActionResult> AddBasket(int id)
        {
            var BasketItems = JsonSerializer.Deserialize<List<BasketAddVm>>(Request.Cookies["basket"] ?? "[]");
            var item = BasketItems.FirstOrDefault(x => x.Id == id);
            if (item is null)
            {
                item = new BasketAddVm(id);
            }
            else
            {
                item.Count++;
            }
            Response.Cookies.Append("basket", JsonSerializer.Serialize(BasketItems));

            return Ok();
        }

    }
}
