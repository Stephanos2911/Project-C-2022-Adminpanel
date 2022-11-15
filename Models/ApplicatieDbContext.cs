using Microsoft.EntityFrameworkCore;
using Project_C.Models.ProductModels;
using Project_C.Models.StoreModels;

namespace AdminApplication.Models
{
    public class ApplicatieDbContext : DbContext
    {
        public ApplicatieDbContext(DbContextOptions<ApplicatieDbContext> options) 
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Store> Stores { get; set; }
    }

}
