namespace WebUniqlo.ViewModel.Products
{
    public class ProductItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public bool IsInStock { get; set; }
    }
}
