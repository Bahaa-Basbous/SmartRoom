using SmartRoom.Entities;

namespace SmartRoom.Repositories
{
    public interface IActionItemRepository
    {
        Task<IEnumerable<ActionItem>> GetAllAsync();
        Task<ActionItem?> GetByIdAsync(int id);
        Task CreateAsync(ActionItem item);
        Task UpdateAsync(ActionItem item);
        Task DeleteAsync(int id);
    }
}
