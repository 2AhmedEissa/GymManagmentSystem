using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_DAL.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.PL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID";
                return RedirectToAction(nameof(Index));
            }

            var session = await _sessionService.GetSessionDetailsAsync(id, ct);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }
    }
}
