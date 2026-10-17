using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
