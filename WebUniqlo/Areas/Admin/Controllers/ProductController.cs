using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUniqlo.DataAccess;
using WebUniqlo.Enums;
using WebUniqlo.Models;
using WebUniqlo.ViewModel.Products;

namespace WebUniqlo.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = nameof(Roles.Admin))]

    public class ProductController(IWebHostEnvironment _env, UniqloDbContext _sql) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _sql.Products.Include(x => x.Category).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _sql.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM pm)
        {
            if (!pm.CoverFile.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("CoverFile", "Image fayli deyil");
            }
            if (pm.CoverFile.Length > 2 * 1024 * 1024)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _sql.Categories.Where(x => !x.IsDeleted).ToListAsync();
                return View();
            }

            string fileName = Path.GetRandomFileName() + Path.GetExtension(pm.CoverFile.FileName);
            using (Stream s = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "products", fileName)))
            {
                await pm.CoverFile.CopyToAsync(s);
            }
            Product product = new Product
            {
                Name = pm.Name,
                Description = pm.Description,
                CostPrice = pm.CostPrice,
                SellPrice = pm.SellPrice,
                Quantity = pm.Quantity,
                CoverFile = fileName,
                Discount = pm.Discount,
                CategoryId = pm.CategoryId,
            };
            await _sql.Products.AddAsync(product);
            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var data = await _sql.Products.FindAsync(id.Value);
            if (data is null) return NotFound();

            _sql.Products.Remove(data);
            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _sql.Products.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = true;
            await _sql.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Show(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _sql.Products.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = false;
            await _sql.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = await _sql.Categories.Where(x => !x.IsDeleted).ToListAsync();
            if (!id.HasValue) return BadRequest();
            var products = await _sql.Products.Where(x => x.Id == id.Value).Select(x => new ProductUpdateVM
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description,
                SellPrice = x.SellPrice,
                Quantity = x.Quantity,
                CostPrice = x.CostPrice,
                Discount = x.Discount,
                CoverFileURL=x.CoverFile,
            }).FirstOrDefaultAsync();
            if (products is null) return BadRequest();
            return View(products);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM pm)
        {

            if (!id.HasValue) return BadRequest();
            if (pm.CoverFile != null)
            {
                if (!pm.CoverFile.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("CoverFile", "Image deyil");
                }
                if (pm.CoverFile.Length > 5 * 1024* 1024)
                {
                    ModelState.AddModelError("CoverFile", "maks 5mb");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _sql.Categories.Where(x => !x.IsDeleted).ToListAsync();
                return View(pm);
            }
            var data = await _sql.Products.Where(x => x.Id == id.Value).FirstOrDefaultAsync();
            if (data is null) return BadRequest();

            if (pm.CoverFile != null)
            {
                string oldName = Path.Combine(_env.WebRootPath, "imgs", "products", data.CoverFile);

                using (Stream s = System.IO.File.Create(oldName))
                {
                    await pm.CoverFile!.CopyToAsync(s);
                }
            }
            data.Name = pm.Name;
            data.Description = pm.Description;
            data.CostPrice = pm.CostPrice;
            data.SellPrice = pm.SellPrice;
            data.Discount = pm.Discount;
            data.Quantity = pm.Quantity;
            data.CategoryId = pm.CategoryId;

            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

    }
}
