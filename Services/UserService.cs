
using Microsoft.EntityFrameworkCore;
using SmartRoom.Entities;
using BCrypt.Net;

using SmartRoom.Repositories;
namespace SmartRoom.Services

{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User?> GetByResetTokenAsync(string token)
        {
            return await _userRepository.GetByResetTokenAsync(token);
        }

        public async Task SaveResetTokenAsync(User user, string token)
        {
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdatePasswordAsync(User user, string newPassword)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> ConfirmResetAsync(string email, string token, string newPassword)
        {
            var user = await _userRepository.GetByEmailAsync(email); // ✅ fix method name
            if (user == null || user.ResetToken != token || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user); // ✅ fix method name
            return true;
        }




        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }


    }
}
