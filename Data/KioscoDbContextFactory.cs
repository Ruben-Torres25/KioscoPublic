using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using KioscoApp.Data;

namespace KioscoApp.Data
{
    public class KioscoDbContextFactory : IDesignTimeDbContextFactory<KioscoDbContext>
    {
        public KioscoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KioscoDbContext>();

            // Usá tu string de conexión real acá
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=KioscoDB;Username=postgres;Password=root");

            return new KioscoDbContext(optionsBuilder.Options);
        }
    }
}
