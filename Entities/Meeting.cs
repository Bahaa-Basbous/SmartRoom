using SmartRoom.Entities;

public class Meeting
{
    public int MeetingID { get; set; }

    public int BookingId { get; set; }  // ✅ FK column
    public Booking Booking { get; set; }  // ✅ Navigation

    public int OrganizerId { get; set; }
    public User Organizer { get; set; }

    public string Title { get; set; }
    public string Agenda { get; set; }
    public DateTime DateTime { get; set; }

    public MoM MoM { get; set; }
}
