namespace CosturaShop.Models
{
  public class ItemCarritoDetallado
  {
    public Producto? Producto { get; set; }
    public Combo? Combo { get; set; }
    public int Cantidad { get; set; }
  }
}