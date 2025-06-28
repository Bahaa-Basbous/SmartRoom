namespace SmartRoom.Dtos
{
    public class CreateMeetingDto
    {
        public int BookingID { get; set; }
        public int OrganizerID { get; set; }
        public string Title { get; set; }
        public string Agenda { get; set; }
        public DateTime DateTime { get; set; }
    }
}
