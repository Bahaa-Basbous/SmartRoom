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

            modelBuilder.Entity<Booking>()
             .HasOne(b => b.Meeting)
             .WithOne(m => m.Booking)
             .HasForeignKey<Meeting>(m => m.BookingId)
             .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Organizer)
                .WithMany()
                .HasForeignKey(m => m.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<MoM>()
                .HasOne(m => m.CreatedBy)
                .WithMany()
                .HasForeignKey(m => m.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);


            // 👈 Prevent cascade
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Meeting> Meetings { get; set; } 
        public DbSet<MoM> MoMs { get; set; }          
        public DbSet<ActionItem> ActionItems { get; set; } 

        
    }
}
