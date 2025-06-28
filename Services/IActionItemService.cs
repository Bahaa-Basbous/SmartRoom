using SmartRoom.Entities;

namespace SmartRoom.Services
{
    public interface IActionItemService
    {
        Task<IEnumerable<ActionItem>> GetAllAsync();
        Task<ActionItem?> GetByIdAsync(int id);
        Task CreateAsync(ActionItem item);
        Task UpdateAsync(ActionItem item);
        Task DeleteAsync(int id);
    }
}
