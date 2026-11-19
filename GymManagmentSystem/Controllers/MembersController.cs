using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // Get BaseUrl/Members/Index
        // Index - List All 

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAlMembersAsync(ct);
            return View(members);
        }


        // Get BaseUrl/Members/Details/{Id}
        // MemberDetails - Displays one Member Profile Details

        // Get BaseUrl/Members/HealthRecordDetails/{Id}
        // HealthRecordDetails - Displays one Health Record Details

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
            } else
            {
                TempData["ErrorMessage"] = "Failed To Create Member";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Member
        // Get BaseUrl/Members/Edit Member
        // Edit - Displays Edit Form

        // Post BaseUrl/Members/Edit/{Id}
        // Edit - Submit Edit Form

        #endregion

        #region Delete Member
        // Get BaseUrl/Members/Delete/{Id}
        // Delete - Displays Delete Form 


        // Post BaseUrl/Members/DeleteConfirmation/{Id}
        // DeleteConfirmation - Submit Delete Form
        #endregion
    }
}
