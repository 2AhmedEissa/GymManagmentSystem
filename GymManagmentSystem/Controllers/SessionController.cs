using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownsAsync(ct: ct);
            return View(new CreateSessionViewModel
            {
                StartDate = DateTime.Now.AddHours(1),
                EndDate = DateTime.Now.AddHours(2)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownsAsync(model.TrainerId, model.CategoryId, ct);
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model, ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(result.PropertyName ?? string.Empty, result.ErrorMessage!);
                await PopulateDropDownsAsync(model.TrainerId, model.CategoryId, ct);
                return View(model);
            }

            TempData["SuccessMessage"] = "Session Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID";
                return RedirectToAction(nameof(Index));
            }

            var result = await _sessionService.GetSessionForUpdateAsync(id, ct);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDownsAsync(result.Data.TrainerId, result.Data.CategoryId, ct);
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownsAsync(model.TrainerId, model.CategoryId, ct);
                return View(model);
            }

            var result = await _sessionService.UpdateSessionAsync(id, model, ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(result.PropertyName ?? string.Empty, result.ErrorMessage!);
                await PopulateDropDownsAsync(model.TrainerId, model.CategoryId, ct);
                return View(model);
            }

            TempData["SuccessMessage"] = "Session Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
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

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _sessionService.DeleteSessionAsync(id, ct);

            if (!result.IsSuccess)
            {
                var session = await _sessionService.GetSessionDetailsAsync(id, ct);
                if (session is not null)
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                    return View("Delete", session);
                }

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to delete session.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Session Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        private async Task PopulateDropDownsAsync(int? selectedTrainerId = null, int? selectedCategoryId = null, CancellationToken ct = default)
        {
            var trainers = await _sessionService.GetTrainersForDropDownAsync(ct);
            var categories = await _sessionService.GetCategoriesForDropDownAsync(ct);

            ViewBag.Trainers = new SelectList(trainers, "Id", "Name", selectedTrainerId);
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);
        }
    }
}
