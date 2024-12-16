using System.ComponentModel.DataAnnotations;

namespace WebUniqlo.ViewModel.Comment
{
    public class CommentCreateVM
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
    }
}
