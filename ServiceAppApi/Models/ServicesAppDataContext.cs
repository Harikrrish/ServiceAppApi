
using Microsoft.EntityFrameworkCore;

namespace ServiceAppApi.Models
{
    public class ServicesAppDataContext : DbContext
    {
        public ServicesAppDataContext(DbContextOptions<ServicesAppDataContext> options) : base(options)    //return function of CreateDbContext method in DesignTimeDbContextFactory.cs 
        {
            Database.Migrate();
        }
        public DbSet<PartyRole> PartyRole { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductServiceMapping> ProductServiceMapping { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<ServiceRequest> ServiceRequest { get; set; }
        public DbSet<ServiceRequestServicesMapping> ServiceRequestServicesMapping { get; set; }
        public DbSet<Service> Service { get; set; }

    }
}
