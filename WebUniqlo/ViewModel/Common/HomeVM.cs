using WebUniqlo.ViewModel.Products;
using WebUniqlo.ViewModel.Sliders;

namespace WebUniqlo.ViewModel.Common
{
    public class HomeVM
    {
        public IEnumerable<SliderItem> Sliders { get; set; }
        public IEnumerable<ProductItemVM> Products { get; set; }
    }
}
