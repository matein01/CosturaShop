using Microsoft.AspNetCore.Mvc;
using CosturaShop.Models;
using CosturaShop.Data;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CosturaShop.Controllers
{
  public class CatalogoController : Controller
  {
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public CatalogoController(ApplicationDbContext Context, IConfiguration configuration)
    {
      this._dbContext = Context;
      this._configuration = configuration;
    }

    public IActionResult Index()
    {
      var productos = _dbContext.Productos.ToList();
      var combos = _dbContext.Combos.Include(c => c.Chaqueta).Include(c => c.Pantalon).ToList();

      ViewBag.Combos = combos;

      return View(productos);
    }

    private List<ItemCarrito> ObtenerCarrito()
    {
      var lista = HttpContext.Session.GetString("Carrito");

      if (lista == null)
      {
        return new List<ItemCarrito>();
      }
      else
      {
        var listaDeserializada = JsonSerializer.Deserialize<List<ItemCarrito>>(lista);
        if (listaDeserializada == null)
        {
          return new List<ItemCarrito>();
        }
        else
        {
          return listaDeserializada;
        }
      }
    }

    private void GuardarCarrito(List<ItemCarrito> carrito)
    {
      var listaSerializada = JsonSerializer.Serialize(carrito);

      HttpContext.Session.SetString("Carrito", listaSerializada);
    }

    public IActionResult AgregarAlCarrito(int? idProducto, int? idCombo)
    {
      var carrito = ObtenerCarrito();

      if (idProducto != null)
      {
        var itemExistente = carrito.FirstOrDefault(item => item.ProductoId == idProducto);

        if (itemExistente == null)
        {
          carrito.Add(new ItemCarrito { ProductoId = idProducto, Cantidad = 1 });
        }
        else
        {
          itemExistente.Cantidad += 1;
        }
      }
      else if(idCombo != null)
      {
        var itemExistente = carrito.FirstOrDefault(item => item.ComboId == idCombo);
        if (itemExistente == null)
        {
          carrito.Add(new ItemCarrito { ComboId = idCombo, Cantidad = 1 });
        }
        else
        {
          itemExistente.Cantidad += 1;
        }
      }
      GuardarCarrito(carrito);
      return RedirectToAction("Index");
    }

    public IActionResult VerCarrito()
    {
      var ItemsCarritoDetallado = ObtenerCarritoDetallado();

      return View(ItemsCarritoDetallado);
    }

    private List<ItemCarritoDetallado> ObtenerCarritoDetallado()
    {
      var carrito = ObtenerCarrito();

      List<ItemCarritoDetallado> ItemsCarritoDetallado = new List<ItemCarritoDetallado>();

      foreach (var item in carrito)
      {
        if (item.ProductoId != null)
        {
          var productoMostrar = _dbContext.Productos.Find(item.ProductoId);

          if (productoMostrar == null)
          {
            continue;
          }

          ItemCarritoDetallado ItemDetallado = new ItemCarritoDetallado { Producto = productoMostrar, Cantidad = item.Cantidad };
          ItemsCarritoDetallado.Add(ItemDetallado);
        }
        else if (item.ComboId != null)
        {
          var comboMostrar = _dbContext.Combos.Find(item.ComboId);
          
          if (comboMostrar == null)
          {
            continue;
          }

          ItemCarritoDetallado ItemDetallado = new ItemCarritoDetallado { Combo = comboMostrar, Cantidad = item.Cantidad };
          ItemsCarritoDetallado.Add(ItemDetallado);
        }
      }

      return ItemsCarritoDetallado;
    }

    [HttpGet]
    public IActionResult Checkout()
    {
      var carritoDetallado = ObtenerCarritoDetallado();
      ViewBag.Carrito = carritoDetallado;
      return View(new DatosCheckout());
    }

    [HttpPost]
    public IActionResult Checkout(DatosCheckout datos)
    {
      var carritoDetallado = ObtenerCarritoDetallado();
      var numero = _configuration["WhatsApp:NumeroCosturero"];

      var mensaje = new StringBuilder();
      mensaje.AppendLine($"Nuevo pedido de {datos.NombreCliente}");
      mensaje.AppendLine($"Numero de telefono: {datos.Telefono}");
      mensaje.AppendLine($"Correo electronico: {datos.Correo}");
      mensaje.AppendLine("Productos:");

      foreach (var item in carritoDetallado)
      {
        if (item.Producto != null)
        {
          mensaje.AppendLine($"- {item.Producto.Nombre} {item.Cantidad} x ${item.Producto.Precio}");
        }
        else if (item.Combo != null)
        {
          mensaje.AppendLine($"- {item.Combo.Nombre} {item.Cantidad} x ${item.Combo.Precio}");
        }
      }

      var mensajeCodificado = Uri.EscapeDataString(mensaje.ToString());

      var url = $"https://wa.me/{numero}?text={mensajeCodificado}";

      return Redirect(url);
    }
    
    public IActionResult EliminarDelCarrito(int? idProducto, int? idCombo)
    {
      var carrito = ObtenerCarrito();

      if (idProducto != null)
      {
        var itemAQuitar = carrito.FirstOrDefault(item => item.ProductoId == idProducto);

        if (itemAQuitar != null)
        {
          if (itemAQuitar.Cantidad > 1)
          {
            itemAQuitar.Cantidad -= 1;
          }
          else
          {
            carrito.Remove(itemAQuitar);
          }
        }
      }
      else if (idCombo != null)
      {
        var itemAQuitar = carrito.FirstOrDefault(item => item.ComboId == idCombo);

        if (itemAQuitar != null)
        {
          if (itemAQuitar.Cantidad > 1)
          {
            itemAQuitar.Cantidad -= 1;
          }
          else
          {
            carrito.Remove(itemAQuitar);
          }
        }
      }

      GuardarCarrito(carrito);
      return RedirectToAction("VerCarrito");
    }
  }
}