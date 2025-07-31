using SmartRoom.Entities;

namespace SmartRoom.Services
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task CreateAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
        Task<List<Booking>> GetPendingBookingsAsync();
        Task UpdateBookingStatusAsync(int bookingId, string status);

    }
}
