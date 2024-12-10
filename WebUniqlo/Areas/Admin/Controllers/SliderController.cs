using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUniqlo.DataAccess;
using WebUniqlo.Enums;
using WebUniqlo.Models;
using WebUniqlo.ViewModel.Products;
using WebUniqlo.ViewModel.Sliders;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Uniqlo.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = nameof(Roles.Admin))]


    public class SliderController(UniqloDbContext _sql, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _sql.Sliders.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {
            if (!slider.File.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("File", "File type must be image");
                return View();
            }
            if (slider.File.Length > 2 * 1024 * 1024) { return View(); }

            if (!ModelState.IsValid) return View();


            string newFileName = Path.GetRandomFileName() + Path.GetExtension(slider.File.FileName);//sekil.jpg

            using (Stream strem = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "Slider", newFileName)))
            {
                await slider.File.CopyToAsync(strem);
            }
            Slider slider1 = new Slider
            {
                ImageUrl = newFileName,
                Link = slider.Link,
                Subtitle = slider.Subtitle,
                Title = slider.Title,
            };
            await _sql.Sliders.AddAsync(slider1);
            await _sql.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var data = await _sql.Sliders.FindAsync(id.Value);
            if (data is null) return NotFound();
            _sql.Sliders.Remove(data);
            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Hide(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _sql.Sliders.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = true;
            await _sql.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Show(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _sql.Sliders.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = false;
            await _sql.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var sliders = await _sql.Sliders.Where(x => x.Id == id.Value).Select(x => new SliderUpdateVM
            {
                Title = x.Title,
                Subtitle = x.Subtitle,
                Link = x.Link,
            }).FirstOrDefaultAsync();
            if (sliders is null) return BadRequest();
            return View(sliders);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM pm)
        {

            if (!id.HasValue) return BadRequest();
            if (pm.ImageUrl != null)
            {
                if (!pm.ImageUrl.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("CoverFile", "Image deyil");
                }
                if (pm.ImageUrl.Length < 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("CoverFile", "Image deyil");
                }
            }
           
            var data = await _sql.Sliders.Where(x => x.Id == id.Value).FirstOrDefaultAsync();
            if (data is null) return BadRequest();

            if (pm.ImageUrl != null)
            {
                string oldName = Path.Combine(_env.WebRootPath, "imgs", "products", data.ImageUrl);

                using (Stream s = System.IO.File.Create(oldName))
                {
                    await pm.ImageUrl!.CopyToAsync(s);
                }
            }
            data.Title = pm.Title;
            data.Subtitle = pm.Subtitle;
            data.Link = pm.Link;

            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

