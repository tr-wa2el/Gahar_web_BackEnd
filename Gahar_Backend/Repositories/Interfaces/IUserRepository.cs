using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
     Task<User?> GetByEmailAsync(string email);
   Task<User?> GetByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
     Task<bool> ExistsByUsernameAsync(string username);
   Task<IEnumerable<string>> GetUserRolesAsync(int userId);
        Task<IEnumerable<string>> GetUserPermissionsAsync(int userId);
        Task<bool> AssignRoleAsync(int userId, string roleName);
   Task<bool> RemoveRoleAsync(int userId, string roleName);
    }
}
