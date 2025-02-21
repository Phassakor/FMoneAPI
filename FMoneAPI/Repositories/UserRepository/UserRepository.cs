using AutoMapper;
using FMoneAPI.Data;
using FMoneAPI.DTOs;
using FMoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FMoneAPI.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<User>> GetTotalUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task AddUserPermissionsAsync(List<UserMenuPermission> permissions)
        {
            _context.UserMenuPermission.AddRange(permissions);
            await _context.SaveChangesAsync();
        }
        public async Task<UserDTO> GetUserById(int id)
        {
            //return await _context.Users.FindAsync(id);
            var users = await _context.Users.FindAsync(id);
            return _mapper.Map<UserDTO>(users);
        }
        public async Task<bool> UpdateUser(UpdateUserRequestDTO updatedUser, string password)
        {
            var getUser = await _context.Users.FindAsync(updatedUser.UserID);
            if (getUser == null) return false;

            getUser.Fname = updatedUser.Fname;
            getUser.Lname = updatedUser.Lname;
            getUser.Username = updatedUser.Username;
            getUser.Email = updatedUser.Email;
            getUser.Phone = updatedUser.Phone;
            getUser.Status = "1";
            getUser.RoleId = updatedUser.RoleId;
            if (!string.IsNullOrWhiteSpace(password))
            {
                getUser.PasswordHash = password;
            }

            _context.Users.Update(getUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var getUser = await _context.Users.FindAsync(id);
            if (getUser == null) return false;

            _context.Users.Remove(getUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateUserPermissionsAsync(
          List<UserMenuPermission> permissionsToAdd,
          List<UserMenuPermission> permissionsToUpdate,
          List<UserMenuPermission> permissionsToRemove)
        {
            if (permissionsToAdd.Any())
            {
                await _context.UserMenuPermission.AddRangeAsync(permissionsToAdd);
            }

            if (permissionsToUpdate.Any())
            {
                _context.UserMenuPermission.UpdateRange(permissionsToUpdate);
            }

            if (permissionsToRemove.Any())
            {
                _context.UserMenuPermission.RemoveRange(permissionsToRemove);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<List<UserMenuPermission>> GetUserPermissionsByUserIdAsync(int userId)
        {
            return await _context.UserMenuPermission
                .Where(ump => ump.UserId == userId)
                .Include(ump => ump.Menu) // โหลดข้อมูล Menu มาด้วย
                .Include(ump => ump.User) // โหลดข้อมูล User มาด้วย
                .ToListAsync();
        }
        public async Task<List<UserMenuPermission>> GetUserPermissionsMenuByUserId(int userId)
        {
            return await _context.UserMenuPermission
            .Where(ump => ump.UserId == userId)
            .Include(ump => ump.Menu) // ดึงข้อมูล Menu มาด้วย
            .ToListAsync();
        }
        public async Task<List<Menus>> GetTotalMenus()
        {
            return await _context.Menus.ToListAsync();
        }
    }
}
