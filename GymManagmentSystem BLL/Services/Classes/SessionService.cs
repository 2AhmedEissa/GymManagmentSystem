using AutoMapper;
using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels.Session;
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
        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);

            if (!sessions.Any()) return [];

            var sessionsViewModel = sessions.OrderByDescending(s => s.StartDate)
                .Select(s => new SessionViewModel()
                {
                    Id = s.Id,
                    Description = s.Description,
                    CategoryName = s.Category.Name,
                    TrainerName = s.Trainer.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Capacity = s.Capacity,
                });
            foreach (var session in sessionsViewModel)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookeSlotsAsync(session.Id, ct);
            }

            return sessionsViewModel;
        }

        public async Task<SessionViewModel> GetSessionDetailsAsync(int sessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId);

            if (session is null) return null;

            var vm = _mapper.Map<SessionViewModel>(session);

            return vm;
        }

        public Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<EditSessionViewModel> GetSessionByIdForEditAsync(int sessionId, EditSessionViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditSessionAsync(EditSessionViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSessionAsync(int sessionId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
