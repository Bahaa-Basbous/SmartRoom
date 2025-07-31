using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SmartRoom.Dtos;
using SmartRoom.Entities;
using SmartRoom.Services;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
namespace SmartRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Employee")] // Only Admin and Employee can use this controller
    public class MoMController : ControllerBase
    {
        private readonly IMoMService _service;
        private readonly IPdfService _pdfService;
        private readonly IWebHostEnvironment _environment;
        public MoMController(IMoMService service, IPdfService pdfService, IWebHostEnvironment environment)
        {
            _service = service;
            _pdfService = pdfService;
            _environment = environment;
        }

        // GET: api/mom
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var moms = await _service.GetAllAsync();

            var momDtos = moms.Select(m => new MoMDto
            {
                MoMID = m.MoMID,
                MeetingID = m.MeetingID,
                MeetingTitle = m.Meeting?.Title ?? "N/A",
                Summary = m.Summary,
                Notes = m.Notes,
                CreatedAt = m.CreatedAt,
                
                CreatedByName = m.CreatedBy?.Name ?? "Unknown",
                ActionItems = m.ActionItems.Select(ai => new ActionItemDto
                {
                    ActionItemID = ai.ActionItemID,
                    Description = ai.Description,
                    DiscussionPoint = ai.DiscussionPoint,
                    Decision = ai.Decision,
                    AssignedToId = ai.AssignedToId,
                    AssignedToName = ai.AssignedTo?.Name ?? "Unassigned",
                    IsCompleted = ai.IsCompleted,
                    DueDate = ai.DueDate
                }).ToList()
            });

            return Ok(momDtos);
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var mom = new MoM
            {
                MeetingID = dto.MeetingID,
                Summary = dto.Summary,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            try
            {
                await _service.CreateAsync(mom);
                return Ok(mom);
            }
            catch (InvalidOperationException ex)
            {
                // Return 409 Conflict if MoM already exists for the meeting
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions if needed
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
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


        [HttpPost("{momId}/action-items")]
        public async Task<IActionResult> AddActionItem(int momId, [FromBody] ActionItemDto dto)
        {
            var mom = await _service.GetByIdAsync(momId);
            if (mom == null)
                return NotFound();

            var actionItem = new ActionItem
            {
                MoMID = momId,
                Description = dto.Description,
                DiscussionPoint = dto.DiscussionPoint,
                Decision = dto.Decision,
                AssignedToId = dto.AssignedToId,
                IsCompleted = dto.IsCompleted,
                DueDate = dto.DueDate
            };

            //await _service.AddActionItemAsync(actionItem);  // Implement this in your service/repository
            return Ok(actionItem);
        }

        

       [HttpGet("{id}/generate-pdf")]
public async Task<IActionResult> GenerateMoMPdf(int id)
{
    var momDto = await _service.GetDtoByIdAsync(id);
    if (momDto == null)
        return NotFound();

    try
    {
        var pdfBytes = _pdfService.GenerateMoMPdf(momDto);
        return File(pdfBytes, "application/pdf", $"MoM_{id}.pdf");
    }
    catch (Exception ex)
    {
        // Log the exception (replace with your logging framework)
        Console.WriteLine($"Error generating PDF for MoM {id}: {ex.Message}");
        return StatusCode(500, "Failed to generate PDF");
    }
}





    }
}
