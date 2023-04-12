using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Data.Services
{
    public class SpecialTagService
    {
        private readonly ApplicationDbContext _db;

        public SpecialTagService(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves a single SpecialTag from the database based on the provided SpecialTagId.
        /// </summary>
        public async Task<SpecialTag> GetSingleSpecialTagAsync(int specialTagId) => await _db.SpecialTags.FindAsync(specialTagId);

        /// <summary>
        /// Gets a list of all special tags from the database asynchronously.
        /// </summary>
        public Task<List<SpecialTag>> GetAllSpecialTagsAsync() => _db.SpecialTags.ToListAsync();

        /// <summary>
        /// Gets a page of SpecialTags from the database.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalCount">The total count of SpecialTags.</param>
        /// <returns>A list of SpecialTags.</returns>
        public List<SpecialTag> GetPagedSpecialTagsAsync(int pageNumber, int pageSize, out int totalCount)
        {
            totalCount = _db.SpecialTags.Count();
            var pages = Task.Run(async () => await _db.SpecialTags.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()).Result;
            return pages;
        }

        /// <summary>
        /// Creates a new SpecialTag in the database.
        /// </summary>
        /// <param name="newSpecialTag">The SpecialTag to be created.</param>
        /// <returns>True if the SpecialTag was created successfully, false otherwise.</returns>
        public async Task<bool> CreateSpecialTagAsync(SpecialTag newSpecialTag)
        {
            if (newSpecialTag is null)
                return false;

            await _db.SpecialTags.AddAsync(newSpecialTag);
            await _db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Updates a SpecialTag in the database.
        /// </summary>
        /// <param name="specialTagsForUpdate">The SpecialTag to be updated.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public async Task<bool> UpdateSpecialTagAsync(SpecialTag specialTagsForUpdate)
        {
            var specialTagFromDb = await _db.SpecialTags.FindAsync(specialTagsForUpdate.Id);

            if (specialTagFromDb is null)
                return false;

            specialTagFromDb.Name = specialTagsForUpdate.Name;
            await _db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Deletes a SpecialTag from the database.
        /// </summary>
        /// <param name="specialTagsForDelete">The SpecialTag to delete.</param>
        /// <returns>True if the SpecialTag was deleted, false otherwise.</returns>
        public async Task<bool> DeleteSpecialTagAsync(SpecialTag specialTagsForDelete)
        {
            var specialTagFromDb = await _db.SpecialTags.FindAsync(specialTagsForDelete.Id);

            if (specialTagFromDb is null)
                return false;

            _db.SpecialTags.Remove(specialTagFromDb);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
