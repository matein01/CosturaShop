using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CosturaShop.Models;

namespace CosturaShop.Data

{
    public class ApplicationDbContext : DbContext
    {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Combo> Combos { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Producto>().Property(x => x.Precio).HasPrecision(10, 2);

      modelBuilder.Entity<Combo>().Property(x => x.Precio).HasPrecision(10, 2);

      modelBuilder.Entity<Combo>().HasOne(c => c.Pantalon).WithMany().HasForeignKey(c => c.PantalonId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Combo>().HasOne(c => c.Chaqueta).WithMany().HasForeignKey(c => c.ChaquetaId).OnDelete(DeleteBehavior.Cascade);
    }

    }
}