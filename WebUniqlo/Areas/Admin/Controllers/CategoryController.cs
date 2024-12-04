using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUniqlo.DataAccess;
using WebUniqlo.Models;
using WebUniqlo.ViewModel.Categories;
using WebUniqlo.ViewModel.Products;

namespace WebUniqlo.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]

    public class CategoryController(UniqloDbContext _sql) : Controller
    {

        public async Task<IActionResult> Index()
        {
            return View(await _sql.Categories.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM cm)
        {
            if (!ModelState.IsValid) return BadRequest();
            Category c = new Category
            {
                Name= cm.Name,
            };
            await _sql.Categories.AddAsync(c);
             _sql.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var data = await _sql.Categories.FindAsync(id.Value);
            if (data is null) return NotFound();
            _sql.Categories.Remove(data);
            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var category = await _sql.Categories.Where(x => x.Id == id.Value).Select(x => new CategoryUpdateVM
            {
                Name = x.Name,
            }).FirstOrDefaultAsync();
            if (category is null) return BadRequest();
            return View(category);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM cm)
        {

            if (!id.HasValue) return BadRequest();
           
            var category = await _sql.Categories.Where(x => x.Id == id.Value).FirstOrDefaultAsync();

            if (category is null) return BadRequest();

            category.Name = cm.Name;


            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Hide(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _sql.Categories.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = true;
            await _sql.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Show(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _sql.Categories.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = false;
            await _sql.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
