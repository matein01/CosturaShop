using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CosturaShop.Models;
using CosturaShop.Data;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Text;

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

    public IActionResult AgregarAlCarrito(int idProducto)
    {
      var carrito = ObtenerCarrito();
      var itemExistente = carrito.FirstOrDefault(item => item.ProductoId == idProducto);

      if (itemExistente == null)
      {
        carrito.Add(new ItemCarrito { ProductoId = idProducto, Cantidad = 1 });
      }
      else
      {
        itemExistente.Cantidad += 1;
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
        var productoMostrar = _dbContext.Productos.Find(item.ProductoId);

        if (productoMostrar == null)
        {
          continue;
        }
        else
        {
          ItemCarritoDetallado ItemDetallado = new ItemCarritoDetallado { Producto = productoMostrar, Cantidad = item.Cantidad };
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
        mensaje.AppendLine($"- {item.Cantidad} {item.Producto.Nombre} = ${item.Producto.Precio}");
      }

      var mensajeCodificado = Uri.EscapeDataString(mensaje.ToString());

      var url = $"https://wa.me/{numero}?text={mensajeCodificado}";
      
      return Redirect(url);
    }
  }
}