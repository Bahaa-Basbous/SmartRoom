using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRoom.Entities;
using SmartRoom.Services;

namespace SmartRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Employee")]
    public class ActionItemController : ControllerBase
    {
        private readonly IActionItemService _service;

        public ActionItemController(IActionItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActionItem item)
        {
            await _service.CreateAsync(item);
            return Ok(item);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can update action items
        public async Task<IActionResult> Update(int id, [FromBody] ActionItem item)
        {
            if (id != item.ActionItemID)
                return BadRequest(new { message = "ID mismatch." });

            await _service.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
