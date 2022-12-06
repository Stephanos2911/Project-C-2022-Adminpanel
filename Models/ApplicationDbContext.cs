using Microsoft.EntityFrameworkCore;
using Project_C.Models;
using Project_C.Models.UserModels;

namespace AdminApplication.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }

        //[Finished]one product has multiple stores, (multiple products are owned by Seniors(not confirmed))
        public DbSet<Product> Products { get; set; }

        //[Finished] a store can be used by multiple products
        public DbSet<Store> Stores { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Employee> Employees { get; set; }


        ////[Finished] every senior has 1 caretaker and multiple products
        //public DbSet<Senior> Seniors { get; set; }
    }

}
