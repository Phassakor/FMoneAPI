using System.ComponentModel.DataAnnotations;

namespace FMoneAPI.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? Fname { get; set; }
        public string? Lname { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }

        public ICollection<UserMenuPermission> UserMenuPermissions { get; set; }
    }
}
