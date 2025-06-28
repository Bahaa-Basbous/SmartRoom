using Microsoft.AspNetCore.Identity;
using SmartRoom.Entities;

using SmartRoom.Repositories;
using SmartRoom.Helpers;

namespace SmartRoom.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenGenerator _tokenGenerator;
        private readonly PasswordHasher<User> _hasher;

        public AuthService(IUserRepository userRepository, TokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Role = request.Role,
                ProfileDetails = "", // You can add more later
            };

            user.PasswordHash = _hasher.HashPassword(user, request.Password);

            await _userRepository.AddAsync(user);
            return true;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null) return null;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed) return null;

            var token = _tokenGenerator.GenerateToken(user);
            return new LoginResponse { Token = token, Role = user.Role, Name = user.Name };
        }
    }
}
