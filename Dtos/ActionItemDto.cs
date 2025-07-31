
namespace SmartRoom.Dtos
{
    public class ActionItemDto
    {
        public int ActionItemID { get; set; }
        public string Description { get; set; }
        public string DiscussionPoint { get; set; }
        public string Decision { get; set; }
        public int? AssignedToId { get; set; }
        public string AssignedToName { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
