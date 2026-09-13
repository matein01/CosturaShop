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
  }
}