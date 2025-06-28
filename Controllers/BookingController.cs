using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRoom.Entities;
using SmartRoom.Services;
using SmartRoom.Dtos;
using System.Security.Claims;

namespace SmartRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Employee")] // ❌ Guests cannot access any booking route
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        // ✅ Admin: view all | Employee: only their own
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAll()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            IEnumerable<Booking> bookings;

            if (currentUserRole == "Admin")
            {
                bookings = await _service.GetAllAsync(); // Admin sees all
            }
            else
            {
                bookings = (await _service.GetAllAsync())
                    .Where(b => b.UserId == currentUserId); // Employee sees only their own
            }

            return Ok(bookings);
        }

        // ✅ Admin: view any | Employee: only their own
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetById(int id)
        {
            var booking = await _service.GetByIdAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (booking.UserId != currentUserId && currentUserRole != "Admin")
                return Forbid(); // Employees can't view others' bookings

            return Ok(booking);
        }

        // ✅ Only Employees or Admins can create a booking
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Optionally enforce: employee can only create for themselves
            if (currentUserRole != "Admin" && dto.UserId != currentUserId)
                return Forbid();

            var booking = new Booking
            {
                RoomId = dto.RoomId,
                UserId = dto.UserId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _service.CreateAsync(booking);
            return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
        }

        // ✅ Only Admins can update
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] Booking booking)
        {
            if (id != booking.BookingId)
                return BadRequest(new { message = "Booking ID mismatch." });

            await _service.UpdateAsync(booking);
            return NoContent();
        }

        // ✅ Only Admins can delete bookings
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
