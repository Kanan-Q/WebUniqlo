using WebUniqlo.Models;

namespace WebUniqlo.ViewModel.Products
{
    public class ProductCreateVM
    {
        public string Name { get; set; }
        public string Description { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public IFormFile CoverFile { get; set; }
        public IEnumerable<IFormFile> OtherFiles { get; set; }
        public int? CategortId { get; set; }
        public Category? Category { get; set; }
    }
}
