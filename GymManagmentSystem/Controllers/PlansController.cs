using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> _planRepo;

        public PlansController(IGenericRepository<Plan> planRepo)
        {
            _planRepo = planRepo;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepo.GetAllAsync(ct: ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planRepo.GetByIdAsync(id, ct);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
