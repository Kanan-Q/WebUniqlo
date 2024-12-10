using System.ComponentModel.DataAnnotations.Schema;
using WebUniqlo.ViewModel.Products;

namespace WebUniqlo.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string CoverFile { get; set; } = null!;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public IEnumerable<ProductImage>? ProductImages { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<ProductRating>? Ratings { get; set; }
    }
}
