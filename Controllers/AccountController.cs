using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CosturaShop.Models;
using CosturaShop.Data;
using Microsoft.AspNetCore.Identity;

namespace CosturaShop.Controllers
{
  public class AccountController : Controller
  {
    private readonly SignInManager<IdentityUser> _dbAccount;

    public AccountController(SignInManager<IdentityUser> _dbAccount)
    {
      this._dbAccount = _dbAccount;
    }

    [HttpGet]
    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
      var resultado = await _dbAccount.PasswordSignInAsync(email, password, false, false);

      if (resultado.Succeeded)
      {
        return RedirectToAction("Index", "Productos");
      }
      else
      {
          return View();
      }
    }
  }
}