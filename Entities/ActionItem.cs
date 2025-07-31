using SmartRoom.Entities;

public class ActionItem
{
    public int ActionItemID { get; set; }
    public int MoMID { get; set; }
    public string Description { get; set; } = string.Empty;
    public string DiscussionPoint { get; set; } = string.Empty;
    public string Decision { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }

    public int? AssignedToId { get; set; }  // <== Ensure this exists

    public User? AssignedTo { get; set; }  // Navigation property
    public bool IsCompleted { get; set; }

    public MoM MoM { get; set; } = null!;
}
