namespace WebUniqlo.Models
{
    public class Comment : BaseEntity
    {
        public string Author { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
