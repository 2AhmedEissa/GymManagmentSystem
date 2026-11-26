using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels.Trainer;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;

namespace GymManagmentSystem_BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);

            if (!trainers.Any()) return [];

            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Specialization,
            });
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);

            if (trainer is null) return null;

            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Specialization = trainer.Specialization,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth,
                Address = trainer.Address is not null
                    ? $"{trainer.Address.BuildingNo} - {trainer.Address.Street} - {trainer.Address.City}"
                    : "N/A"
            };
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Specialization = model.Specialization,
                Address = new Address
                {
                    BuildingNo = model.Address.BuildingNo,
                    Street = model.Address.Street,
                    City = model.Address.City
                }
            };

            var emailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email, ct);
            var phoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone, ct);

            if (emailExists || phoneExists) return false;

            _unitOfWork.GetRepository<Trainer>().AddAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<EditTrainerViewModel?> GetTrainerByIdForEditAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);

            if (trainer is null) return null;

            return new EditTrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialization = trainer.Specialization,
                BuildingNumber = trainer.Address?.BuildingNo ?? 0,
                Street = trainer.Address?.Street ?? string.Empty,
                City = trainer.Address?.City ?? string.Empty,
            };
        }

        public async Task<bool> EditTrainerAsync(int trainerId, EditTrainerViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);

            if (trainer is null) return false;

            var emailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email && t.Id != trainerId, ct);
            var phoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct);

            if (emailExists || phoneExists)
            {
                return false;
            }

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Specialization = model.Specialization;
            trainer.UpdatedAt = DateTime.Now;

            trainer.Address ??= new Address();
            trainer.Address.BuildingNo = model.BuildingNumber;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;

            _unitOfWork.GetRepository<Trainer>().UpdateAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);

            if (trainer is null) return false;

            var hasUpcomingSessions = await _unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);

            if (hasUpcomingSessions) return false;

            _unitOfWork.GetRepository<Trainer>().DeleteAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}