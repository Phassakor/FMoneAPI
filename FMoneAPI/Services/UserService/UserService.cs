using AutoMapper;
using FMoneAPI.DTOs;
using FMoneAPI.Models;
using FMoneAPI.Repositories.UserRepository;

namespace FMoneAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<User>> GetTotalUsers()
        {
            return await _userRepository.GetTotalUsers();
        }
        public async Task<(bool success, string message)> CreateUserAsync(CreateUserRequestDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return (false, "Invalid user data");

            // ✅ 1. เช็คว่ามี User ซ้ำหรือไม่
            if (await _userRepository.UserExistsAsync(request.Username))
                return (false, "Username already exists");

            // ✅ 2. สร้าง User ใหม่
            var newUser = new User
            {
                Fname = request.Fname,
                Lname = request.Lname,
                Username = request.Username,
                PasswordHash = request.Password,
                Email = request.Email,
                Phone = request.Phone,
                Status = "1",
                RoleId = request.RoleId,
                CreateDate = DateTime.UtcNow
            };

            newUser = await _userRepository.CreateUserAsync(newUser);

            // ✅ 3. กำหนดสิทธิ์ให้ User
            var userPermissions = request.MenuPermissions.Select(mp => new UserMenuPermission
            {
                UserId = newUser.UserID,
                MenuId = mp.MenuId,
                CanAccess = mp.CanAccess
            }).ToList();

            await _userRepository.AddUserPermissionsAsync(userPermissions);

            return (true, "User created successfully with permissions");
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return null;
            return user;
        }
        public async Task<bool> UpdateUser(UpdateUserRequestDTO updatedUser, string password)
        {
            return await _userRepository.UpdateUser(updatedUser, password);
        }
        public async Task<bool> DeleteUser(int id)
        {
            return await (_userRepository.DeleteUser(id));
        }
        public async Task<(bool success, string message)> UpdateUserPermissionsAsync(UpdateUserPermissionDTO request)
        {
            var existingPermissions = await _userRepository.GetUserPermissionsByUserIdAsync(request.UserID);
            if (existingPermissions == null)
            {
                return (false, "UserID not found");
            }
            var existingPermissionsDict = existingPermissions.ToDictionary(p => p.MenuId);

            var newPermissions = request.MenuPermissions.Select(mp => new UserMenuPermission
            {
                UserId = request.UserID,
                MenuId = mp.MenuId,
                CanAccess = mp.CanAccess
            }).ToList();
            var permissionsToAdd = new List<UserMenuPermission>();
            var permissionsToUpdate = new List<UserMenuPermission>();
            var permissionsToRemove = new List<UserMenuPermission>();

            foreach (var newPermission in newPermissions)
            {
                if (existingPermissionsDict.TryGetValue(newPermission.MenuId, out var existingPermission))
                {
                    // ถ้ามีอยู่แล้ว แต่ค่าต่างกัน ให้ทำการอัปเดต
                    if (existingPermission.CanAccess != newPermission.CanAccess)
                    {
                        existingPermission.CanAccess = newPermission.CanAccess;
                        permissionsToUpdate.Add(existingPermission);
                    }

                    // นำออกจาก Dictionary เพื่อใช้หาสิทธิ์ที่ต้องลบ
                    existingPermissionsDict.Remove(newPermission.MenuId);
                }
                else
                {
                    // ถ้าเป็นรายการใหม่ ให้เพิ่ม
                    permissionsToAdd.Add(newPermission);
                }
            }
            permissionsToRemove.AddRange(existingPermissionsDict.Values);

            await _userRepository.UpdateUserPermissionsAsync(permissionsToAdd, permissionsToUpdate, permissionsToRemove);
      
            return (true, "User Update permissions successfully");
        }
        public async Task<List<UserMenuPermission>> GetUserPermissionsMenuByUserId(int userId)
        {
            return await _userRepository.GetUserPermissionsMenuByUserId(userId);
        }
        public async Task<List<Menus>> GetTotalMenus()
        {
            return await _userRepository.GetTotalMenus();
        }
        public async Task<object?> Login(string username, string password)
        {
            return await _userRepository.Login(username, password);
        }
    }
}
