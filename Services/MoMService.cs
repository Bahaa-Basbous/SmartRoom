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
    }
}
