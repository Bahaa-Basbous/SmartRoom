using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRoom.Dtos;
using SmartRoom.Entities;
using SmartRoom.Services;
using System.Security.Claims;

namespace SmartRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Employee")] // Only Admin and Employee can use this controller
    public class MoMController : ControllerBase
    {
        private readonly IMoMService _service;

        public MoMController(IMoMService service)
        {
            _service = service;
        }

        // GET: api/mom
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET: api/mom/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mom = await _service.GetByIdAsync(id);
            if (mom == null)
                return NotFound();

            return Ok(mom);
        }

        // POST: api/mom
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMoMDto dto)
        {
            var mom = new MoM
            {
                MeetingID = dto.MeetingID,
                Summary = dto.Summary,
                Notes = dto.Notes,
                FilePath = dto.FilePath,
                CreatedAt = DateTime.UtcNow
            };

            await _service.CreateAsync(mom);
            return Ok(mom);
        }

        // PUT: api/mom/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can edit MoM
        public async Task<IActionResult> Update(int id, [FromBody] MoM mom)
        {
            if (id != mom.MoMID)
                return BadRequest(new { message = "MoM ID mismatch." });

            await _service.UpdateAsync(mom);
            return NoContent();
        }

        // DELETE: api/mom/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can delete
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
