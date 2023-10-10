using TestTaskUser.Helpers;
using TestTaskUser.Models;

namespace TestTaskUser.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> ReadAll(ParameterPack parameters);
        Task<User?> Read(int id);
        Task<User?> Read(string email);
        Task<List<User>> Create(User user);
        Task<List<User>?> Update(int id, User user);
        Task<List<User>?> Delete(int id);
        Task<List<User>?> AddRole(int userId, int roleId);
    }
}
