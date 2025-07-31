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
        .Include(m => m.CreatedBy)
        .Include(m => m.ActionItems)
            .ThenInclude(ai => ai.AssignedTo)
        .ToListAsync();

        public async Task<MoM?> GetByIdAsync(int id)
            => await _context.MoMs
                .Include(m => m.Meeting)
                .Include(m => m.CreatedBy)
                .Include(m => m.ActionItems)
                    .ThenInclude(ai => ai.AssignedTo)
                .FirstOrDefaultAsync(m => m.MoMID == id);


        public async Task CreateAsync(MoM mom)
        {
            var existingMoM = await _context.MoMs
                .FirstOrDefaultAsync(m => m.MeetingID == mom.MeetingID);

            if (existingMoM != null)
            {
                throw new InvalidOperationException("A MoM for this meeting already exists.");
            }

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
        public async Task<MoM?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.MoMs
                .Include(m => m.Meeting)
                .Include(m => m.CreatedBy)
                .Include(m => m.ActionItems)
                    .ThenInclude(ai => ai.AssignedTo)
                .FirstOrDefaultAsync(m => m.MoMID == id);
        }


    }
}
