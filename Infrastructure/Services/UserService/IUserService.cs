using Domain.DTO;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();

        Task<User> GetUserByIdAsync(string userId);

        Task<User> AddUser(UserDTO userDTO);

        Task DeleteUserAsync(string userId);

    }
}