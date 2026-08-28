using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosturaShop.Models
{
  public enum TipoProducto
  {
    Pantalon,
    Chaqueta
  }
    public class Producto
    {
      public int Id {get; set;} 
      public string Nombre  {get; set;} = string.Empty;
      public string? Descripcion  {get; set;}
      public decimal Precio {get; set;} 
      public string? Material {get; set;} 
      public string? Talla {get; set;} 
      public string? Color {get; set;} 
      public TipoProducto Tipo  {get; set;} 
      public string? ImagenUrl {get; set;}
    }
}