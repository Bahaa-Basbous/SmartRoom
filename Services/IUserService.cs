using SmartRoom.Entities;

namespace SmartRoom.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByResetTokenAsync(string token);
        Task SaveResetTokenAsync(User user, string token);
        Task UpdatePasswordAsync(User user, string newPassword);
        Task<bool> ConfirmResetAsync(string email, string token, string newPassword);

        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
