using SmartRoom.Entities;
using SmartRoom.Repositories;

namespace SmartRoom.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;

        public BookingService(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Booking booking)
        {
            var existing = await _repository.GetAllAsync();

            bool conflict = existing.Any(b =>
                b.RoomId == booking.RoomId &&
                booking.StartTime < b.EndTime &&
                booking.EndTime > b.StartTime
            );

            if (conflict)
                throw new InvalidOperationException("This room is already booked for the selected time slot.");

            booking.CreatedAt = DateTime.UtcNow;
            booking.Status = "Pending";
            await _repository.AddAsync(booking);
        }


        public async Task UpdateAsync(Booking booking)
        {
            await _repository.UpdateAsync(booking);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<List<Booking>> GetPendingBookingsAsync()
        {
            var allBookings = await _repository.GetAllAsync();
            return allBookings.Where(b => b.Status == "Pending").ToList();
        }

        public async Task UpdateBookingStatusAsync(int bookingId, string status)
        {
            var booking = await _repository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            // Optional: Validate status values here (e.g., only "Approved" or "Rejected")
            if (status != "Approved" && status != "Rejected")
                throw new ArgumentException("Invalid status value.");

            booking.Status = status;
            await _repository.UpdateAsync(booking);
        }

    }
}
