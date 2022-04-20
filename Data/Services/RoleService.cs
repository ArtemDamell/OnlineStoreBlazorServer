using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorShop.Data.Services
{
    public class RoleService
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _db.Roles.ToListAsync();
        }

        public async Task<bool> CreateNewRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return false;

            var res = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (res.Succeeded)
            {
                await _db.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<IdentityRole> ReadRoleByNameAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                return null;

            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is not null)
                return role;
            else
                return null;
        }

        public async Task<bool> UpdateRoleByNameAsync(IdentityRole roleForUpdate)
        {
            var currRole = await _roleManager.FindByIdAsync(roleForUpdate.Id);

            if (currRole is null)
                return false;

            await _roleManager.UpdateAsync(roleForUpdate);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
                return false;

            await _roleManager.DeleteAsync(role);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
