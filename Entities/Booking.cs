using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRoom.Entities
{
    [PrimaryKey("BookingId")]
    public class Booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BookingId { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Pending"; // Default status is Pending
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("User")]

        public virtual User User { get; set; } = null!;

        // Navigation properties can be added if using an ORM like Entity Framework
        // public virtual Room Room { get; set; }
        // public virtual User User { get; set; }
    }
}
