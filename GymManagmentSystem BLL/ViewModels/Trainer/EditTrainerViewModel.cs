using GymManagmentSystem_DAL.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagmentSystem_BLL.ViewModels.Trainer
{
    public class EditTrainerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone must be a valid 11-digit Egyptian number.")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Building number is required.")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [StringLength(30, ErrorMessage = "Street cannot exceed 30 characters.")]
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(30, ErrorMessage = "City cannot exceed 30 characters.")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "Specialization is required.")]
        public Specialties Specialization { get; set; }
    }
}