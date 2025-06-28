namespace SmartRoom.Entities
{
    public class ActionItem
    {
        public int ActionItemID { get; set; }
        public int MoMID { get; set; }
        public string Description { get; set; }
        public int AssignedTo { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }

        public MoM MoM { get; set; }
        public User AssignedUser { get; set; }
    }

}
