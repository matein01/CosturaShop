using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosturaShop.Models
{
  public class ItemCarritoDetallado
  {
    public Producto Producto { get; set; } = new Producto();
    public int Cantidad { get; set; }
  }
}