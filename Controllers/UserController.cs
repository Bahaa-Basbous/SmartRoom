using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartRoom.Dtos;
using SmartRoom.Entities;
using SmartRoom.Services;
using System.Security.Claims;
namespace SmartRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            await _userService.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var user = await _userService.GetByIdAsync(userId);
                if (user == null) return NotFound();

                return Ok(new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Error in GetMyProfile: " + ex.Message);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpPut("me/update")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userService.GetByIdAsync(userId);
            if (user == null) return NotFound();

            user.Name = dto.Name;
            user.Email = dto.Email;
            await _userService.UpdateAsync(user);

            return NoContent();
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var user = await _userService.GetByEmailAsync(dto.Email);
            if (user == null)
                return NotFound(new { message = "User not found." });

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);

            if (result == PasswordVerificationResult.Failed)
                return BadRequest(new { message = "Old password is incorrect." });

            user.PasswordHash = hasher.HashPassword(user, dto.NewPassword);
            await _userService.UpdateAsync(user);

            return Ok(new { message = "Password changed successfully." });
        }







    }
}
