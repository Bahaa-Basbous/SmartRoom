namespace SmartRoom.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int BookingId { get; set; }
        public bool Seen { get; set; } = false;
        public DateTime CreatedAt { get; set; }

        public Booking Booking { get; set; }
    }
}
