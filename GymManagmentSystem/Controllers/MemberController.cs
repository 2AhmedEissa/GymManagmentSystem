using GymManagmentSystem_BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // Get BaseUrl/Members/Index
        // Index - List All 

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }


        #region Member Details
        // Get BaseUrl/Members/Details/{Id}
        // MemberDetails - Displays one Member Profile Details
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID";
                return RedirectToAction(nameof(Index));
            }

            var member = await _memberService.GetMemberDetailsAsync(id, ct);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // Get BaseUrl/Members/HealthRecordDetails/{Id}
        // HealthRecordDetails - Displays one Health Record Details 
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecord = await _memberService.GetHealthRecordDetailsAsync(id, ct);

            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(healthRecord);
        }

        #endregion

        #region Create Member

        // Get BaseUrl/Members/Create
        // Create - Displays Create Form

        public IActionResult Create()
        {
            return View();
        }

        // Post BaseUrl/Member/Create Member
        // Create - Submit Create Form 
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _memberService.CreateMemberAsync(model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Succesfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Member";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Member
        // Get BaseUrl/Members/Edit Member
        // Edit - Displays Edit Form
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID";
                return RedirectToAction(nameof(Index));
            }

            var member = await _memberService.GetMemberToUpdateAsync(id, ct);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        // Post BaseUrl/Members/Edit/{Id}
        // Edit - Submit Edit Form
        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _memberService.UpdateMemberDetailsAsync(id, model, ct);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to update member. Email or phone number may already be in use.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Member Updated Successfully ";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Member
        // Get BaseUrl/Members/Delete/{Id}
        // Delete - Displays Delete Form 
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // Post BaseUrl/Members/DeleteConfirmation/{Id}
        // DeleteConfirmation - Submit Delete Form
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _memberService.DeleteMemberAsync(id, ct);

            if (result) TempData["SuccessMessage"] = "Member Deleted Successfully";
            else TempData["ErrorMessage"] = "Failed To Delete Member";

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
