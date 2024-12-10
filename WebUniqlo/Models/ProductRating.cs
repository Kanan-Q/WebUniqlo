using System.ComponentModel.DataAnnotations;

namespace WebUniqlo.Models
{
    public class ProductRating
    {
        public int Id { get; set; }
        [Range (0,5)]
        public int Rating{ get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }


    }
}
