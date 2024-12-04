namespace WebUniqlo.ViewModel.Sliders
{
    public class SliderUpdateVM
    {
        public string Title { get; set; } = null!;
        public string Subtitle { get; set; } = null!;
        public string? Link { get; set; }
        public string? CoverImageUrl { get; set; } = null!;
        public IFormFile ImageUrl { get; set; } = null!;
    }
}
