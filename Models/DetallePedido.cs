using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosturaShop.Models
{
    public class DetallePedido
    {
      public int Id { get; set; }
      public int PedidoId { get; set; }
    public Pedido Pedido { get; set; } = new Pedido();
      public int Cantidad { get; set; }
      public int? ProductoId { get; set; }
      public Producto? Producto { get; set; }
      public int? ComboId { get; set; }
      public Combo? Combo { get; set; }
    }
}