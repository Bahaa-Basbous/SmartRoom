namespace SmartRoom.Dtos
{
    public class CreateMoMDto
    {
        public int MeetingID { get; set; }
        public string Summary { get; set; }
        public string Notes { get; set; }
        public string? FilePath { get; set; }
    }
}
