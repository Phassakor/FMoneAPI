using FMoneAPI.DTOs;
using FMoneAPI.Models;

namespace FMoneAPI.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetTotalUsers();
        Task<(bool success, string message)> CreateUserAsync(CreateUserRequestDTO request);
        Task<UserDTO> GetUserById(int id);
        Task<bool> UpdateUser(UpdateUserRequestDTO updatedUser, string password);
        Task<bool> DeleteUser(int id);
        Task<(bool success, string message)> UpdateUserPermissionsAsync(UpdateUserPermissionDTO request);
        Task<List<UserMenuPermission>> GetUserPermissionsMenuByUserId(int userId);
        Task<List<Menus>> GetTotalMenus();
        Task<object?> Login(string username, string password);
    }
}
