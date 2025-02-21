namespace FMoneAPI.DTOs
{
    public class CreateUserRequestDTO
    {
        public string? Fname { get; set; }
        public string? Lname { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public List<MenuPermission> MenuPermissions { get; set; }
    }
    public class UpdateUserPermissionDTO
    {
        public int UserID { get; set; }
        public List<MenuPermission> MenuPermissions { get; set; }
    }
    public class MenuPermission
    {
        public int MenuId { get; set; }
        public bool CanAccess { get; set; }
    }
}
