using Domain.DTO;
using Domain.Models;
using System.Threading.Tasks;

namespace Infrastructure.Services.UserService
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string userId);

        Task<User> AddUser(UserDTO userDTO);
    }
}