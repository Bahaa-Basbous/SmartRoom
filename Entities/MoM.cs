namespace SmartRoom.Entities
{
    public class MoM
    {
        public int MoMID { get; set; }
        public int MeetingID { get; set; }
        public string Summary { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public Meeting Meeting { get; set; }
        public ICollection<ActionItem> ActionItems { get; set; } = new List<ActionItem>();

    }

}
