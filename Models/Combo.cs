using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosturaShop.Models
{
    public class Combo
    {
      public int Id { get; set; }
      public string Nombre { get; set; } = string.Empty;
      public string? Descripcion { get; set; }
      public decimal Precio { get; set; }
      public int ChaquetaId { get; set; }
      public int PantalonId { get; set; }
      public Producto Chaqueta { get; set; } = new Producto();
      public Producto Pantalon { get; set; } = new Producto();
    }
}