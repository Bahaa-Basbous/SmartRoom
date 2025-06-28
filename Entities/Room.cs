namespace SmartRoom.Entities
{
    public class Room
    {
        public int RoomID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public string Features { get; set; }

        //public ICollection<Booking> Bookings { get; set; }
    }

}
