using GymManagmentSystem.DbContexts;
using GymManagmentSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GymManagmentSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext _dbcontext;

        public PlansController()
        {
            _dbcontext = new GymDbContext();
        }
        public async Task<IActionResult> Index()
        {
            var plans = await _dbcontext.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await _dbcontext.Plans.FindAsync(id);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
