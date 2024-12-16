using System.ComponentModel.DataAnnotations;

namespace WebUniqlo.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        public string Description { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
