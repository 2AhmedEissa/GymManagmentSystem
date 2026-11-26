using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels.Trainer;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.PL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region Get

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID";
                return RedirectToAction(nameof(Index));
            }

            var trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _trainerService.CreateTrainerAsync(model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed To Create Trainer";
            return View(model);
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID";
                return RedirectToAction(nameof(Index));
            }

            var trainer = await _trainerService.GetTrainerByIdForEditAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _trainerService.EditTrainerAsync(id, model, ct);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to update trainer. Email or phone number may already be in use.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Trainer Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID";
                return RedirectToAction(nameof(Index));
            }

            var trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainerService.DeleteTrainerAsync(id, ct);

            if (result) TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else TempData["ErrorMessage"] = "Failed To Delete Trainer";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}