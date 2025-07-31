using SmartRoom.Dtos;
using SmartRoom.Entities;
using SmartRoom.Repositories;

namespace SmartRoom.Services
{
    public class MoMService : IMoMService
    {
        private readonly IMoMRepository _repository;

        public MoMService(IMoMRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<MoM>> GetAllAsync() => _repository.GetAllAsync();
        public Task<MoM?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task CreateAsync(MoM mom) => _repository.CreateAsync(mom);
        public Task UpdateAsync(MoM mom) => _repository.UpdateAsync(mom);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
        public async Task<MoMDto?> GetDtoByIdAsync(int id)
        {
            var mom = await _repository.GetByIdWithDetailsAsync(id);
            if (mom == null)
                return null;

            return new MoMDto
            {
                MoMID = mom.MoMID,
                MeetingID = mom.MeetingID,
                MeetingTitle = mom.Meeting?.Title ?? "N/A",
                Summary = mom.Summary,
                Notes = mom.Notes,
                CreatedAt = mom.CreatedAt,
                
                CreatedByName = mom.CreatedBy?.Name ?? "Unknown",
                ActionItems = mom.ActionItems.Select(ai => new ActionItemDto
                {
                    ActionItemID = ai.ActionItemID,
                    Description = ai.Description,
                    DiscussionPoint = ai.DiscussionPoint,
                    Decision = ai.Decision,
                    AssignedToId = ai.AssignedToId,
                    AssignedToName = ai.AssignedTo?.Name ?? "Unassigned",
                    IsCompleted = ai.IsCompleted,
                    DueDate = ai.DueDate
                }).ToList()
            };
        }

    }
}
