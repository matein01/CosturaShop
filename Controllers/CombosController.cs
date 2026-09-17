using Microsoft.AspNetCore.Mvc;
using CosturaShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CosturaShop.Models;

namespace CosturaShop.Controllers
{
  [Authorize]
  public class CombosController : Controller
  {
    private readonly ApplicationDbContext _dbContext;

    public CombosController(ApplicationDbContext Context)
    {
      this._dbContext = Context;
    }

    public IActionResult Index()
    {
      var combos = _dbContext.Combos.Include(c => c.Chaqueta).Include(c => c.Pantalon).ToList();

      return View(combos);
    }

    [HttpGet]
    public IActionResult Create()
    {
      var chaquetas = _dbContext.Productos.Where(p => p.Tipo == TipoProducto.Chaqueta);
      var pantalones = _dbContext.Productos.Where(p => p.Tipo == TipoProducto.Pantalon);

      var listaChaquetas = new SelectList(chaquetas, "Id", "Nombre");
      var listaPantalones = new SelectList(pantalones, "Id", "Nombre");

      ViewBag.Chaquetas = listaChaquetas;
      ViewBag.Pantalones = listaPantalones;

      return View();
    }

    [HttpPost]
    public IActionResult Create(Combo combo)
    {
      combo.Chaqueta = null;
      combo.Pantalon = null;

      _dbContext.Combos.Add(combo);
      _dbContext.SaveChanges();

      return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int idComboEditar)
    {
      var chaquetas = _dbContext.Productos.Where(p => p.Tipo == TipoProducto.Chaqueta);
      var pantalones = _dbContext.Productos.Where(p => p.Tipo == TipoProducto.Pantalon);

      var listaChaquetas = new SelectList(chaquetas, "Id", "Nombre");
      var listaPantalones = new SelectList(pantalones, "Id", "Nombre");

      ViewBag.Chaquetas = listaChaquetas;
      ViewBag.Pantalones = listaPantalones;

      var comboEditar = _dbContext.Combos.Find(idComboEditar);

      return View(comboEditar);
    }

    [HttpPost]
    public IActionResult Edit(Combo combo)
    {
      combo.Chaqueta = null;
      combo.Pantalon = null;

      _dbContext.Combos.Update(combo);
      _dbContext.SaveChanges();

      return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Delete(int idComboEliminar)
    {
      var comboEliminar = _dbContext.Combos.Include(c => c.Chaqueta).Include(c => c.Pantalon).FirstOrDefault(c => c.Id == idComboEliminar);

      return View(comboEliminar);
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int Id)
    {
      var comboEliminar = _dbContext.Combos.Include(c => c.Chaqueta).Include(c => c.Pantalon).FirstOrDefault(c => c.Id == Id);
      if (comboEliminar == null)
      {
        return RedirectToAction("Index");
      }
      else
      {
        _dbContext.Combos.Remove(comboEliminar);
        _dbContext.SaveChanges();

        return RedirectToAction("Index");
      }
    }
  }
}