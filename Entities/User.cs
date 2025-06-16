using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRoom.Entities
{
    [PrimaryKey("Id")]

    public class User
    {
        [Key]
        public int Id { get; set; } // ✅ EF will automatically treat this as PK

        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string ProfileDetails { get; set; }
    }
}