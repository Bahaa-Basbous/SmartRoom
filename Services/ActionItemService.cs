using SmartRoom.Entities;
using SmartRoom.Repositories;

namespace SmartRoom.Services
{
    public class ActionItemService : IActionItemService
    {
        private readonly IActionItemRepository _repository;

        public ActionItemService(IActionItemRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ActionItem>> GetAllAsync() => _repository.GetAllAsync();
        public Task<ActionItem?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task CreateAsync(ActionItem item) => _repository.CreateAsync(item);
        public Task UpdateAsync(ActionItem item) => _repository.UpdateAsync(item);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
