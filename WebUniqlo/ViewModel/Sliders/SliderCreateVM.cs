using System.ComponentModel.DataAnnotations;

namespace WebUniqlo.ViewModel.Sliders
{
    public class SliderCreateVM
    {
        [MaxLength(32, ErrorMessage = "32 simvoldan cox ola bilmez")]
        [Required]
        public string Title { get; set; }
        [Required]
        public string? Subtitle { get; set; }
        public string? Link { get; set; }
        [Required(ErrorMessage = "Fayl secilmeyib")]
        public IFormFile File { get; set; }
    }
}
