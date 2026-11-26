using AutoMapper;
using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels.Trainer;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;

namespace GymManagmentSystem_BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);

            if (!trainers.Any()) return [];

            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);

            if (trainer is null) return null;

            return _mapper.Map<TrainerViewModel>(trainer);
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var emailExists = await repo.AnyAsync(t => t.Email == model.Email, ct);
            var phoneExists = await repo.AnyAsync(t => t.Phone == model.Phone, ct);

            if (emailExists || phoneExists) return false;

            var trainer = _mapper.Map<Trainer>(model);

            repo.AddAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<EditTrainerViewModel?> GetTrainerByIdForEditAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);

            if (trainer is null) return null;

            return _mapper.Map<EditTrainerViewModel>(trainer);
        }

        public async Task<bool> EditTrainerAsync(int trainerId, EditTrainerViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await repo.GetByIdAsync(trainerId, ct);

            if (trainer is null) return false;

            var emailExists = await repo.AnyAsync(t => t.Email == model.Email && t.Id != trainerId, ct);
            var phoneExists = await repo.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct);

            if (emailExists || phoneExists)
            {
                return false;
            }

            _mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.Now;

            repo.UpdateAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await repo.GetByIdAsync(trainerId, ct);

            if (trainer is null) return false;

            var hasUpcomingSessions = await _unitOfWork.GetRepository<Session>()
                .AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);

            if (hasUpcomingSessions) return false;

            repo.DeleteAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}