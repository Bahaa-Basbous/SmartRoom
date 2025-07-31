using SmartRoom.Dtos;
public class MoMDto
{
    public int MoMID { get; set; }
    public int MeetingID { get; set; }
    public string MeetingTitle { get; set; }
    public string Summary { get; set; } = string.Empty; // initialize to empty string

    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string CreatedByName { get; set; }
    public List<ActionItemDto> ActionItems { get; set; } = new List<ActionItemDto>();
}