using Microsoft.EntityFrameworkCore;
using SmartRoom.Entities;

namespace SmartRoom.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Booking)
                .WithMany()
                .HasForeignKey(m => m.BookingID)
                .OnDelete(DeleteBehavior.Restrict); // 👈 Prevent cascade

            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Organizer)
                .WithMany()
                .HasForeignKey(m => m.OrganizerID)
                .OnDelete(DeleteBehavior.Restrict); // 👈 Prevent cascade
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Meeting> Meetings { get; set; }  // If you're using it
        public DbSet<MoM> MoMs { get; set; }          // If exists
        public DbSet<ActionItem> ActionItems { get; set; } // If exists

        
    }
}
