using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRoom.Entities;
using SmartRoom.Services;
using SmartRoom.Dtos;

namespace SmartRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _service;
        private readonly IBookingService _bookingService;

        public MeetingController(IMeetingService service, IBookingService bookingService)
        {
            _service = service;
            _bookingService = bookingService;
        }

        // ✅ Accessible to authenticated users (Employee/Admin)
        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<ActionResult<IEnumerable<Meeting>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // ✅ Accessible to authenticated users
        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<ActionResult<Meeting>> GetById(int id)
        {
            var meeting = await _service.GetByIdAsync(id);
            if (meeting == null)
                return NotFound(new { message = "Meeting not found." });

            return Ok(meeting);
        }

        // ✅ Only Employees or Admins can schedule a meeting
        [HttpPost]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<ActionResult> Create([FromBody] CreateMeetingDto dto)
        {
            var booking = await _bookingService.GetByIdAsync(dto.BookingID);
            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            if (booking.Status != "Approved")
                return BadRequest(new { message = "Cannot create meeting from a booking that is not approved." });

            // Check if Meeting already exists for this booking
            var existingMeeting = await _service.GetByBookingIdAsync(dto.BookingID);
            if (existingMeeting != null)
                return BadRequest(new { message = "A meeting for this booking already exists." });

            var meeting = new Meeting
            {
                Title = dto.Title,
                Agenda = dto.Agenda,
                BookingId = dto.BookingID,
                OrganizerId = dto.OrganizerID,
                DateTime = dto.DateTime
            };

            await _service.CreateAsync(meeting);
            return CreatedAtAction(nameof(GetById), new { id = meeting.MeetingID }, meeting);
        }


        // ✅ Only Admins can update meetings
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] Meeting meeting)
        {
            if (id != meeting.MeetingID)
                return BadRequest(new { message = "Meeting ID mismatch." });

            await _service.UpdateAsync(meeting);
            return NoContent();
        }

        // ✅ Only Admins can delete meetings
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
