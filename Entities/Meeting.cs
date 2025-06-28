namespace SmartRoom.Entities
{
    public class Meeting
    {
        public int MeetingID { get; set; }
        public int BookingID { get; set; }
        public int OrganizerID { get; set; }
        public string Title { get; set; }
        public string Agenda { get; set; }
        public DateTime DateTime { get; set; }

        public Booking Booking { get; set; }
        public User Organizer { get; set; }
        public MoM MoM { get; set; }
        //public ICollection<Attendee> Attendees { get; set; }
    }

}
