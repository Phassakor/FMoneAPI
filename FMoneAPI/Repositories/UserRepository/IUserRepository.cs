using FMoneAPI.DTOs;
using FMoneAPI.Models;

namespace FMoneAPI.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetTotalUsers();
        Task<bool> UserExistsAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task AddUserPermissionsAsync(List<UserMenuPermission> permissions);
        Task<UserDTO> GetUserById(int id);
        Task<bool> UpdateUser(UpdateUserRequestDTO updatedUser, string password);
        Task<bool> DeleteUser(int id);
        Task<List<UserMenuPermission>> GetUserPermissionsByUserIdAsync(int userId);
        Task UpdateUserPermissionsAsync(
          List<UserMenuPermission> permissionsToAdd,
          List<UserMenuPermission> permissionsToUpdate,
          List<UserMenuPermission> permissionsToRemove);
        Task<List<UserMenuPermission>> GetUserPermissionsMenuByUserId(int userId);
        Task<List<Menus>> GetTotalMenus();
    }
}
