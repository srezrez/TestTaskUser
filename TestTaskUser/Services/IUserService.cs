using TestTaskUser.Helpers;
using TestTaskUser.Models;

namespace TestTaskUser.Services
{
    public interface IUserService
    {
        Task<List<User>> ReadAll(ParameterPack parameters);
        Task<User?> Read(int id);
        Task<User?> Login(string email, string password);
        Task<List<User>> Create(User user, string password);
        Task<List<User>?> Update(int id, User user);
        Task<List<User>?> Delete(int id);
        Task<List<User>?> AddRole(int userId, int roleId);
    }
}
