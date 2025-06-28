using SmartRoom.Data;
using SmartRoom.Entities;
using Microsoft.EntityFrameworkCore;

namespace SmartRoom.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly ApplicationDbContext _context;

        public MeetingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Meeting>> GetAllAsync()
            => await _context.Meetings.ToListAsync();

        public async Task<Meeting?> GetByIdAsync(int id)
            => await _context.Meetings.FindAsync(id);

        public async Task CreateAsync(Meeting meeting)
        {
            await _context.Meetings.AddAsync(meeting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Meeting meeting)
        {
            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting != null)
            {
                _context.Meetings.Remove(meeting);
                await _context.SaveChangesAsync();
            }
        }
    }
}
