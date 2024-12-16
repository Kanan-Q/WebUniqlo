using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebUniqlo.Areas.Admin.Controllers;
using WebUniqlo.Models;

namespace WebUniqlo.DataAccess
{
    public class UniqloDbContext:IdentityDbContext<User>
    {
        public DbSet<Slider> Sliders {  get; set; }
        public DbSet<Product> Products {  get; set; }
        public DbSet<Category> Categories {  get; set; }
        public DbSet<ProductImage> ProductImages {  get; set; }
        public DbSet<Tag> Tags {  get; set; }
        public DbSet<ProductRating> ProductRatings {  get; set; }
        public DbSet<Comment> Comments {  get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Category>(x => x.Property(y => y.IsDeleted).HasDefaultValueSql("false"));
        //    base.OnModelCreating(modelBuilder);
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-IOJO115;Database=UniqloWebDB;Trusted_Connection=True;TrustServerCertificate=True");
        //    base.OnConfiguring(optionsBuilder);
        //}
        public UniqloDbContext(DbContextOptions opt) : base(opt) { }
    }
}
