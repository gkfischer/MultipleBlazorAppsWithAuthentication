using Microsoft.AspNetCore.Mvc;

namespace MultipleBlazorApps.Server.Controllers;

public class HomeController : Controller
{
    #region Methods

    public IActionResult Index()
    {
        return View();
    }

    #endregion Methods
}