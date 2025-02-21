using AutoMapper;
using FMoneAPI.DTOs;
using FMoneAPI.Models;

namespace FMoneAPI
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>(); // แมปจาก UserDTO -> User
            CreateMap<User, UserDTO>(); // แมปจาก User -> UserDTO
        }
    }
}
