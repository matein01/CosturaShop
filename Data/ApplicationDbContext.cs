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

    }
}