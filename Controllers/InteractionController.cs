using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using guidebot_api.Models;
using Microsoft.AspNetCore.Authorization;

namespace guidebot_api.Controllers;


[Route("{controller}")]
public class InteractionController : Controller
{
    private readonly ILogger<InteractionController> _logger;

    public InteractionController(ILogger<InteractionController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    [HttpPost()]
    public IActionResult LogInteraction()
    {
        return NotFound();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
