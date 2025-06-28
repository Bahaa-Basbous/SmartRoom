using SmartRoom.Entities;

namespace SmartRoom.Repositories
{
    public interface IMoMRepository
    {
        Task<IEnumerable<MoM>> GetAllAsync();
        Task<MoM?> GetByIdAsync(int id);
        Task CreateAsync(MoM mom);
        Task UpdateAsync(MoM mom);
        Task DeleteAsync(int id);
    }
}
