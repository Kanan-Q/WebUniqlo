using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebUniqlo.DataAccess;
using WebUniqlo.ViewModel.Basket;

namespace WebUniqlo.ViewComponents
{
    public class HeaderViewComponent(UniqloDbContext _sql) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var BasketIds = JsonSerializer.Deserialize<List<BasketAddVm>>(Request.Cookies["basket"] ?? "[]");
            var data = await _sql.Products.Where(x => BasketIds.Select(y => y.Id).Any(y => y == x.Id)).Select(x => new ProductBasketItemVm
            {
                Id = x.Id,
                Name = x.Name,
                Discount = x.Discount,
                ImageUrl = x.CoverFile,
                SellPrice = x.SellPrice,

            }).ToListAsync();
            foreach (var item in data)
            {
                item.Count = BasketIds.FirstOrDefault(x => x.Id == item.Id)!.Count;
            }
            return View(data);
        }
    }
}
