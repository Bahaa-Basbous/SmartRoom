namespace SmartRoom.Dtos
{
    public class ChangePasswordDto
    {
        public string Email { get; set; } = string.Empty;
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
