using Microsoft.EntityFrameworkCore;
using Project_C.Models.ProductModels;
using Project_C.Models.StoreModels;

namespace AdminApplication.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Store> Stores { get; set; }
    }

}
