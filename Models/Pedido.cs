using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosturaShop.Models
{
    public class Pedido
    {
      public int Id { get; set; }
      public string NombreCliente { get; set; } = string.Empty;
      public string Telefono { get; set; } = string.Empty;
      public string Correo { get; set; } = string.Empty;
      public DateTime Fecha { get; set; }
      public List<DetallePedido> DetallesPedidos { get; set; } = new List<DetallePedido>();
    }
}