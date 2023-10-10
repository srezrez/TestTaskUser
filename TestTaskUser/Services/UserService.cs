using TestTaskUser.Data;
using TestTaskUser.DTO;
using TestTaskUser.Helpers;
using TestTaskUser.Models;
using TestTaskUser.Repository;

namespace TestTaskUser.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>?> AddRole(int userId, int roleId)
        {
            if(userId < 0 || roleId < 0) throw new ArgumentException("Request contains invalid userId or roleId parameters");
            
            return await _userRepository.AddRole(userId, roleId);
        }

        public async Task<List<User>> Create(User user, string password)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            
            return await _userRepository.Create(user);
        }

        public async Task<List<User>?> Delete(int id)
        {
            if (id < 0) throw new ArgumentException("Request contains invalid id parameter");
            
            return await _userRepository.Delete(id);
        }

        public async Task<User?> Read(int id)
        {
            if (id < 0) throw new ArgumentException("Request contains invalid id parameter");
            var user = await _userRepository.Read(id);
            
            return user;
        }

        public async Task<User?> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Request contains invalid email parameter");

            var user = await _userRepository.Read(email);
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password)) throw new ArgumentException("Wrong password");
            
            return user;
        }

        public async Task<List<User>> ReadAll(ParameterPack parameters)
        {
            var users = await _userRepository.ReadAll(parameters);
            return users;
        }

        public async Task<List<User>?> Update(int id, User user)
        {
            if (id < 0) throw new ArgumentException("Request contains invalid id parameter");
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var users = await _userRepository.Update(id, user);
            
            return users;
        }
    }
}
