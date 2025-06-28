namespace SmartRoom.Dtos
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
    