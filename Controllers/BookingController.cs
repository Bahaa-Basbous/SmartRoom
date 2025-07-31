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

            try
            {
                await _service.CreateAsync(booking);
                return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
            }
            catch (InvalidOperationException ex)
            {
                // ⛔️ This returns a 400 BadRequest with your custom message
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
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



        // Get pending bookings - Admin only
        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetPendingBookings()
        {
            var pendingBookings = await _service.GetPendingBookingsAsync();
            return Ok(pendingBookings);
        }

        // Update booking status - Admin only
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] UpdateBookingStatusDto dto)
        {
            try
            {
                await _service.UpdateBookingStatusAsync(id, dto.Status);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Booking not found." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("pending/count")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> GetPendingCount()
        {
            var bookings = await _service.GetAllAsync();
            var pendingCount = bookings.Count(b => b.Status == "Pending");
            return Ok(pendingCount);
        }
        // Example: Get only approved bookings
        // GET: api/Booking/approved
        [HttpGet("approved")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetApprovedBookings()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            IEnumerable<Booking> bookings = await _service.GetAllAsync();

            var approvedBookings = bookings.Where(b => b.Status == "Approved");

            if (currentUserRole != "Admin")
            {
                approvedBookings = approvedBookings.Where(b => b.UserId == currentUserId);
            }

            return Ok(approvedBookings);
        }







    }
}
