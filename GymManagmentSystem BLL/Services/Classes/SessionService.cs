using AutoMapper;
using GymManagmentSystem_BLL.Common.Results;
using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels.Session;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;

namespace GymManagmentSystem_BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);

            if (!sessions.Any()) return [];

            return _mapper.Map<List<SessionViewModel>>(sessions.OrderByDescending(s => s.StartDate));
        }

        public async Task<SessionViewModel?> GetSessionDetailsAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithDetailsAsync(sessionId, ct);

            if (session is null) return null;

            return _mapper.Map<SessionViewModel>(session);
        }

        public async Task<ServiceResult> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            var validationResult = await ValidateSessionAsync(
                model.TrainerId, model.CategoryId, model.StartDate, model.EndDate, model.Capacity, ct);

            if (!validationResult.IsSuccess)
                return validationResult;

            var session = _mapper.Map<Session>(model);
            _unitOfWork.SessionRepository.AddAsync(session);
            var saved = await _unitOfWork.SaveChangesAsync(ct);

            return saved > 0
                ? ServiceResult.Success()
                : ServiceResult.Failure("Failed to save session.");
        }

        public async Task<ServiceResult<UpdateSessionViewModel>> GetSessionForUpdateAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(id, ct);

            if (session is null)
                return ServiceResult<UpdateSessionViewModel>.Failure("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return ServiceResult<UpdateSessionViewModel>.Failure("Cannot edit a session that has already started or completed.");

            var vm = _mapper.Map<UpdateSessionViewModel>(session);
            return ServiceResult<UpdateSessionViewModel>.Success(vm);
        }

        public async Task<ServiceResult> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(id, ct);

            if (session is null)
                return ServiceResult.Failure("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return ServiceResult.Failure("Cannot edit a session that has already started or completed.");

            var validationResult = await ValidateSessionAsync(
                model.TrainerId, model.CategoryId, model.StartDate, model.EndDate, model.Capacity, ct);

            if (!validationResult.IsSuccess)
                return validationResult;

            _mapper.Map(model, session);
            session.Id = id;
            session.UpdatedAt = DateTime.Now;

            _unitOfWork.SessionRepository.UpdateAsync(session);
            var saved = await _unitOfWork.SaveChangesAsync(ct);

            return saved > 0
                ? ServiceResult.Success()
                : ServiceResult.Failure("Failed to update session.");
        }

        public async Task<ServiceResult> DeleteSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithDetailsAsync(id, ct);

            if (session is null)
                return ServiceResult.Failure("Session not found.");

            if (session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now)
                return ServiceResult.Failure("Cannot delete a session that is currently ongoing.");

            if (session.Bookings != null && session.Bookings.Any())
                return ServiceResult.Failure("Cannot delete a session that has active bookings.");

            var trackedSession = await _unitOfWork.SessionRepository.GetByIdAsync(id, ct);
            if (trackedSession is null)
                return ServiceResult.Failure("Session not found.");

            _unitOfWork.SessionRepository.DeleteAsync(trackedSession);
            var saved = await _unitOfWork.SaveChangesAsync(ct);

            return saved > 0
                ? ServiceResult.Success()
                : ServiceResult.Failure("Failed to delete session.");
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        private async Task<ServiceResult> ValidateSessionAsync(
            int trainerId,
            int categoryId,
            DateTime startDate,
            DateTime endDate,
            int capacity,
            CancellationToken ct)
        {
            var trainerExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Id == trainerId, ct);
            if (!trainerExists)
                return ServiceResult.Failure("Selected trainer does not exist.", nameof(CreateSessionViewModel.TrainerId));

            var categoryExists = await _unitOfWork.GetRepository<Category>().AnyAsync(c => c.Id == categoryId, ct);
            if (!categoryExists)
                return ServiceResult.Failure("Selected category does not exist.", nameof(CreateSessionViewModel.CategoryId));

            if (startDate <= DateTime.Now)
                return ServiceResult.Failure("Start date must be in the future.", nameof(CreateSessionViewModel.StartDate));

            if (endDate <= startDate)
                return ServiceResult.Failure("End date must be after the start date.", nameof(CreateSessionViewModel.EndDate));

            if (capacity < 1 || capacity > 25)
                return ServiceResult.Failure("Capacity must be between 1 and 25.", nameof(CreateSessionViewModel.Capacity));

            return ServiceResult.Success();
        }
    }
}
