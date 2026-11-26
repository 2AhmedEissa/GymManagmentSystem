using GymManagmentSystem_BLL.Services.Classes;
using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels.Plan;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planService.GetAllPlansAsync(ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanDetailsAsync(id, ct);

            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        #region Edit
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan ID";
                return RedirectToAction(nameof(Index));
            }

            var plan = await _planService.GetPlanToUpdateAsync(id, ct);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found or cannot be edited because it has active memberships.";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditPlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _planService.UpdatePlanDetailsAsync(id, model, ct);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to update plan.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Plan Updated Successfully ";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Toggle(int id, CancellationToken ct)
        {
            var isToggled = await _planService.TogglePlanAsync(id, ct);

            if (!isToggled)
            {
                TempData["ErrorMessage"] = "Failed to update plan status. Active plans with active memberships cannot be deactivated.";
            }
            else
            {
                TempData["SuccessMessage"] = "Plan status updated successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
