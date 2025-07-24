using Microsoft.EntityFrameworkCore;
using KioscoApp.Models;

namespace KioscoApp.Data
{
    public class KioscoDbContext : DbContext
    {
        public KioscoDbContext(DbContextOptions<KioscoDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
    }
}
