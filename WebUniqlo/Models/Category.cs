namespace WebUniqlo.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IEnumerable<Product>? Products { get; set; }
    }
}
