using Microsoft.EntityFrameworkCore;
using SmartRoom.Data;
using SmartRoom.Entities;

namespace SmartRoom.Repositories
{
    public class ActionItemRepository : IActionItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ActionItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActionItem>> GetAllAsync()
            => await _context.ActionItems
                             .Include(a => a.MoM)
                             .Include(a => a.AssignedTo)
                             .ToListAsync();

        public async Task<ActionItem?> GetByIdAsync(int id)
            => await _context.ActionItems
                             .Include(a => a.MoM)
                             .Include(a => a.AssignedTo)
                             .FirstOrDefaultAsync(a => a.ActionItemID == id);

        public async Task CreateAsync(ActionItem item)
        {
            await _context.ActionItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ActionItem item)
        {
            _context.ActionItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.ActionItems.FindAsync(id);
            if (item != null)
            {
                _context.ActionItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
