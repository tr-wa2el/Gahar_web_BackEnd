using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _dbSet.AnyAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUserPermissionsAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> AssignRoleAsync(int userId, string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) return false;

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = role.Id
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRoleAsync(int userId, string roleName)
        {
            var userRole = await _context.UserRoles
                .Include(ur => ur.Role)
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.Role.Name == roleName);

            if (userRole == null) return false;

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
