namespace FMoneAPI.DTOs
{
    public class UpdateUserRequestDTO
    {
        public int UserID { get; set; }
        public string? Fname { get; set; }
        public string? Lname { get; set; }
        public string? Username { get; set; }
        public string? Status { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
    }
}
