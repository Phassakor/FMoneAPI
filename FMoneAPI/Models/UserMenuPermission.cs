using System.Text.Json.Serialization;

namespace FMoneAPI.Models
{
    public class UserMenuPermission
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? MenuId { get; set; }
        public bool CanAccess { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        
        [JsonIgnore]
        public Menus Menu { get; set; }
    }
}
