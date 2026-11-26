using System.ComponentModel.DataAnnotations;

namespace GymManagmentSystem_BLL.ViewModels.Plan
{
    public class EditPlanViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plan name is required.")]
        [StringLength(50, ErrorMessage = "Plan name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days.")]
        [Display(Name = "Duration (Days)")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 99999999.99, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}