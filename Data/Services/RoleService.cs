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

        /// <summary>
        /// Gets a list of all roles from the database.
        /// </summary>
        public Task<List<IdentityRole>> GetAllRolesAsync() => _db.Roles.ToListAsync();

        /// <summary>
        /// Creates a new role with the given name.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        /// <returns>True if the role was created successfully, false otherwise.</returns>
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

        /// <summary>
        /// Reads a role by its name from the role manager.
        /// </summary>
        /// <param name="roleName">The name of the role to read.</param>
        /// <returns>The role with the given name, or null if no such role exists.</returns>
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

        /// <summary>
        /// Updates a role by name asynchronously.
        /// </summary>
        /// <param name="roleForUpdate">The role to be updated.</param>
        /// <returns>A boolean value indicating whether the role was successfully updated.</returns>
        public async Task<bool> UpdateRoleByNameAsync(IdentityRole roleForUpdate)
        {
            var currRole = await _roleManager.FindByIdAsync(roleForUpdate.Id);

            if (currRole is null)
                return false;

            await _roleManager.UpdateAsync(roleForUpdate);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a role by its name.
        /// </summary>
        /// <param name="roleName">The name of the role to delete.</param>
        /// <returns>True if the role was deleted, false otherwise.</returns>
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
