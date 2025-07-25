using Microsoft.EntityFrameworkCore;
using KioscoApp.Models;

namespace KioscoApp.Data
{
    public class KioscoDbContext : DbContext
    {
        public KioscoDbContext(DbContextOptions<KioscoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<MovimientoCaja> MovimientosCaja { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

    }
}
