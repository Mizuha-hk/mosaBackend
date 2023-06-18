using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using mosaCupBackend.Models.DbModels;
namespace mosaCupBackend.Data
{
    public class mosaCupDbContext : DbContext
    {
        public mosaCupDbContext(DbContextOptions<mosaCupDbContext> options) : base(options) { }

        public DbSet<userData> UserData { get; set; } = default!;

        public DbSet<follow> Follow { get; set; } = default!;
        
        public DbSet<post> Post { get; set; } = default!;
        
        public DbSet<like> Like { get; set; } = default!;

        public DbSet<notification> Notification { get; set; } = default!;
    }

/*    public class mosaCupDbContextFactory : IDesignTimeDbContextFactory<mosaCupDbContext>
    {
        public mosaCupDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<mosaCupDbContext>();
            optionsBuilder.UseSqlServer("connectionString");
            return new mosaCupDbContext(optionsBuilder.Options);
        }
    }*/
}
