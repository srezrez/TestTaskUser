using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using System.Linq.Expressions;
using TestTaskUser.Data;
using TestTaskUser.Enums;
using TestTaskUser.Helpers;
using TestTaskUser.Models;

namespace TestTaskUser.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<User>> Create(User user)
        {
            var role = await _dataContext.Roles.FindAsync(UserRole.User);
            if(role == null)
            {
                throw new ObjectNotFoundException($"Role {UserRole.User} not found");
            }
            user.Roles.Add(role);
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return await _dataContext.Users.Include(u => u.Roles).ToListAsync();
        }

        public async Task<List<User>?> Delete(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            if (user is null)
                throw new ObjectNotFoundException($"User with id {id} not found");

            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();

            return await _dataContext.Users.ToListAsync();
        }

        public async Task<User?> Read(int id)
        {
            var user = await _dataContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                throw new ObjectNotFoundException($"User with id {id} not found");

            return user;
        }

        public async Task<List<User>> ReadAll(ParameterPack parameters)
        {
            var result = _dataContext.Users.AsQueryable();

            //Filtering
            if(!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm;
                result = _dataContext.Users.Where(u =>
                    u.Id.ToString() == searchTerm
                    || u.Name.Contains(searchTerm)
                    || u.Age.ToString() == searchTerm
                    || u.Email.Contains(searchTerm)
                    || u.Roles.Any(r => r.Name.Contains(searchTerm)));
            }

            //Sorting
            if(!string.IsNullOrWhiteSpace(parameters?.Sorting?.Field))
            {
                var keySelector = GetSortingProperty(parameters.Sorting);
                if (parameters?.Sorting?.Order == Enums.SortingOrder.Descending)
                {
                    result = result.OrderByDescending(keySelector);
                }
                else
                {
                    result = result.OrderBy(keySelector);
                }
            }    
            
            // Pagination
            if(parameters?.Pagination != null)
            {
                return await result
                .Skip((parameters.Pagination.PageNumber - 1) * parameters.Pagination.PageSize)
                .Take(parameters.Pagination.PageSize)
                .Include(u => u.Roles).ToListAsync();
            }
            
            return await result.Include(u => u.Roles).ToListAsync();
        }

        private static Expression<Func<User, object>> GetSortingProperty(Sorting sorting)
        {
            Expression<Func<User, object>> keySelector = sorting.Field switch
            {
                "id" => user => user.Id,
                "name" => user => user.Name,
                "age" => user => user.Age,
                "email" => user => user.Email,
                _ => user => user.Id,
            };

            return keySelector;
        }

        public async Task<List<User>?> Update(int id, User user)
        {
            var currentUser = await _dataContext.Users.FindAsync(id);
            if (currentUser is null)
                throw new ObjectNotFoundException($"User with id {id} not found");

            currentUser.Name = user.Name;
            currentUser.Age = user.Age;
            currentUser.Email = user.Email;
            currentUser.Password = user.Password;

            await _dataContext.SaveChangesAsync();

            return await _dataContext.Users.Include(u => u.Roles).ToListAsync();
        }

        public async Task<List<User>?> AddRole(int userId, int roleId)
        {
            var user = await _dataContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new ObjectNotFoundException($"User with id {userId} not found");
            var role = await _dataContext.Roles.FirstOrDefaultAsync(r => (int)r.UserRoleId == roleId);
            if (role is null) throw new ObjectNotFoundException($"Role with id {roleId} not found");
            if (user.Roles.Contains(role)) throw new ArgumentException("User already has this role");
            user.Roles.Add(role);
            await _dataContext.SaveChangesAsync();

            return await _dataContext.Users.Include(u => u.Roles).ToListAsync();
        }

        public async Task<User?> Read(string email)
        {
            var user = await _dataContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
                throw new ObjectNotFoundException($"User with email {email} not found");

            return user;
        }
    }
}
