using Domain.DTO;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IOnlineStoreReadRepository<User> _userReadRepository;
        private readonly IOnlineStoreWriteRepository<User> _userWriteRepository;

        public UserService(IOnlineStoreReadRepository<User> userReadRepository,
            IOnlineStoreWriteRepository<User> userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            Guid resultId;
            var isValid = !string.IsNullOrWhiteSpace(userId) ? Guid.TryParse(userId, out resultId) : throw new ArgumentNullException("No User ID was provided.");
            
            if (!isValid)
            {
                throw new InvalidOperationException($"Invalid format of User ID: {userId} provided.");
            }

            var user = await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.UserId == resultId) ??
                throw new ArgumentException($"User does not exist. Incorrect User ID: {userId} provided.");

            return user;
        }

        private async Task<User> GetUserByEmailAsync(string email)
        {
            email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentNullException("No email was provided.");

            var user = await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> AddUser(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ArgumentNullException("User must not be null.");
            }

            if (await GetUserByEmailAsync(userDTO.Email) != null)
            {
                throw new ArgumentException($"User already exists with this email address: {userDTO.Email}.");
            }

            User user = new User()
            {
                UserId = Guid.NewGuid(),
                FirstName = !string.IsNullOrWhiteSpace(userDTO.FirstName) ? userDTO.FirstName : throw new ArgumentNullException($"No {nameof(userDTO.FirstName)}was provided."),
                LastName = !string.IsNullOrWhiteSpace(userDTO.LastName) ? userDTO.LastName : throw new ArgumentNullException($"No {nameof(userDTO.LastName)}was provided."),
                Email = userDTO.Email,
                Password = !string.IsNullOrWhiteSpace(userDTO.Password) ? userDTO.Password : throw new ArgumentNullException($"No {nameof(userDTO.Password)}was provided.")
            };
            user.PartitionKey = user.UserId.ToString();

            return await _userWriteRepository.AddAsync(user);
        }
    }
}
