using GymManagmentSystem_BLL.ViewModels.Trainer;

namespace GymManagmentSystem_BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);
        Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default);
        Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);
        Task<EditTrainerViewModel?> GetTrainerByIdForEditAsync(int trainerId, CancellationToken ct = default);
        Task<bool> EditTrainerAsync(int trainerId, EditTrainerViewModel model, CancellationToken ct = default);
        Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken ct = default);
    }
}