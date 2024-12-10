using WebUniqlo.Models;

namespace WebUniqlo.ViewModel.Products
{
    public class ProductUpdateVM
    {
         public string Name { get; set; }
        public string Description { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string? CoverFileURL {  get; set; }   
        public IFormFile CoverFile { get; set; }
        //public IEnumerable<string>? OtherFileURLs { get; set; }
        //public IEnumerable<IFormFile> OtherFiles { get; set; }
        public int? CategoryId { get; set; }
    }
}
