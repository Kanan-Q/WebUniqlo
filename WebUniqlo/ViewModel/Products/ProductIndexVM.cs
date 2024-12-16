using WebUniqlo.Models;

namespace WebUniqlo.ViewModel.Products
{
    public class ProductIndexVM
    {
        public List<ProductItemVM> Products { get; set; }
        public List<CategoryAndCount> Categories { get; set; }

        public class CategoryAndCount()
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }

        }
    }
}
