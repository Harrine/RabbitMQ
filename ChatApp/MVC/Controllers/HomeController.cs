using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login(){
        return View();
    }


    [HttpPost]
    public IActionResult Login(string username)
    {
        Console.WriteLine(username);
        if (string.IsNullOrEmpty(username))
        {
            return View();
        }
        // ViewBag.Username = username;
        HttpContext.Session.SetString("Username", username);
        return RedirectToAction("Index", "Home");
    }

}
