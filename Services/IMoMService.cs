using SmartRoom.Entities;

namespace SmartRoom.Services
{
    public interface IMoMService
    {
        Task<IEnumerable<MoM>> GetAllAsync();
        Task<MoM?> GetByIdAsync(int id);
        Task CreateAsync(MoM mom);
        Task UpdateAsync(MoM mom);
        Task DeleteAsync(int id);
        Task<MoMDto> GetDtoByIdAsync(int id);

    }
}
