using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CosturaShop.Models;
using CosturaShop.Data;

namespace CosturaShop.Controllers
{
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
  }
}