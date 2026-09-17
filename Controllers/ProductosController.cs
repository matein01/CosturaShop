using Microsoft.AspNetCore.Mvc;
using CosturaShop.Models;
using CosturaShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CosturaShop.Controllers
{
  [Authorize]
  public class ProductosController : Controller
  {
    private readonly ApplicationDbContext _dbContext;

    public ProductosController(ApplicationDbContext Context)
    {
      this._dbContext = Context;
    }

    public IActionResult Index()
    {
      var productos = _dbContext.Productos.ToList();
      var combos = _dbContext.Combos.Include(c => c.Chaqueta).Include(c => c.Pantalon).ToList();

      ViewBag.Combos = combos;

      return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Create(Producto producto)
    {
      _dbContext.Productos.Add(producto);
      _dbContext.SaveChanges();

      return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int idProductoEditar)
    {
      var productoEditar = _dbContext.Productos.Find(idProductoEditar);

      return View(productoEditar);
    }

    [HttpPost]
    public IActionResult Edit(Producto producto)
    {
      _dbContext.Productos.Update(producto);
      _dbContext.SaveChanges();

      return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int idProductoEliminar)
    {
      var productoEliminar = _dbContext.Productos.Find(idProductoEliminar);

      return View(productoEliminar);
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int Id)
    {
      var productoEliminar = _dbContext.Productos.Find(Id);
      if (productoEliminar == null)
      {
        return RedirectToAction("Index");
      }
      else
      {
        _dbContext.Productos.Remove(productoEliminar);
        _dbContext.SaveChanges();

        return RedirectToAction("Index");
      }
    }
  }
}