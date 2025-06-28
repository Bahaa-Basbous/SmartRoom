using SmartRoom.Entities;

namespace SmartRoom.Services
{
    public interface IMeetingService
    {
        Task<IEnumerable<Meeting>> GetAllAsync();
        Task<Meeting?> GetByIdAsync(int id);
        Task CreateAsync(Meeting meeting);
        Task UpdateAsync(Meeting meeting);
        Task DeleteAsync(int id);
    }
}
