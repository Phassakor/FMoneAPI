namespace FMoneAPI.Models
{
    public class Menus
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentId { get; set; }

        public ICollection<UserMenuPermission> UserMenuPermissions { get; set; }
    }
}
