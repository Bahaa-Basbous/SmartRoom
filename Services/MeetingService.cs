using SmartRoom.Entities;
using SmartRoom.Repositories;

namespace SmartRoom.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _repository;

        public MeetingService(IMeetingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Meeting>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Meeting?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Meeting meeting)
        {
            await _repository.CreateAsync(meeting);
        }

        public async Task UpdateAsync(Meeting meeting)
        {
            await _repository.UpdateAsync(meeting);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
