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

namespace CosturaShop.Controllers
{
  public class CatalogoController : Controller
  {
    private readonly ApplicationDbContext _dbContext;

    public CatalogoController(ApplicationDbContext Context)
    {
      this._dbContext = Context;
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
      var carrito = ObtenerCarrito();

      List<ItemCarritoDetallado> ItemsCarritoDetallado = new List<ItemCarritoDetallado>();

      foreach (var item in carrito)
      {
        var productoMostrar = _dbContext.Productos.Find(item.ProductoId);

        if (productoMostrar == null)
        {
          return RedirectToAction("Index");
        }
        else
        {
          ItemCarritoDetallado ItemDetallado = new ItemCarritoDetallado { Producto = productoMostrar, Cantidad = item.Cantidad };
          ItemsCarritoDetallado.Add(ItemDetallado);
        }
      }

      return View(ItemsCarritoDetallado);
    }
  }
}