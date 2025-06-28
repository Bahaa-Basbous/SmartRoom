using Microsoft.EntityFrameworkCore;
using SmartRoom.Data;
using SmartRoom.Entities;

namespace SmartRoom.Repositories
{
    public class MoMRepository : IMoMRepository
    {
        private readonly ApplicationDbContext _context;

        public MoMRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MoM>> GetAllAsync()
            => await _context.MoMs
                .Include(m => m.Meeting)
                .Include(m => m.ActionItems)
                .ToListAsync();

        public async Task<MoM?> GetByIdAsync(int id)
            => await _context.MoMs
                .Include(m => m.Meeting)
                .Include(m => m.ActionItems)
                .FirstOrDefaultAsync(m => m.MoMID == id);

        public async Task CreateAsync(MoM mom)
        {
            await _context.MoMs.AddAsync(mom);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MoM mom)
        {
            _context.MoMs.Update(mom);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mom = await _context.MoMs.FindAsync(id);
            if (mom != null)
            {
                _context.MoMs.Remove(mom);
                await _context.SaveChangesAsync();
            }
        }
    }
}
