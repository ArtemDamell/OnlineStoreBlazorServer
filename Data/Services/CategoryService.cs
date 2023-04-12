using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Data.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves a single category from the database based on the provided categoryId.
        /// </summary>
        public async Task<Category> GetSingleCategoryAsync(int categoryId) => await _db.Categories.FindAsync(categoryId);

        /// <summary>
        /// Retrieves a list of all categories from the database asynchronously.
        /// </summary>
        public Task<List<Category>> GetAllCategoriesAsync() => _db.Categories.ToListAsync();

        /// <summary>
        /// Gets a list of categories from the database, paged according to the given page number and page size.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalCount">The total count of categories.</param>
        /// <returns>A list of categories.</returns>
        public List<Category> GetPagedCategoriesAsync(int pageNumber, int pageSize, out int totalCount)
        {
            totalCount = _db.Categories.Count();
            var pages = Task.Run(async () => await _db.Categories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()).Result;
            return pages;
        }

        /// <summary>
        /// Creates a new category in the database.
        /// </summary>
        /// <param name="newCategory">The category to be created.</param>
        /// <returns>True if the category was created successfully, false otherwise.</returns>
        public async Task<bool> CreateCategoryAsync(Category newCategory)
        {
            if (newCategory is null)
                return false;

            await _db.Categories.AddAsync(newCategory);
            await _db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Updates a category in the database.
        /// </summary>
        /// <param name="categoryForUpdate">The category to be updated.</param>
        /// <returns>True if the category was updated, false otherwise.</returns>
        public async Task<bool> UpdateCategoryAsync(Category categoryForUpdate)
        {
            var categoryFromDb = await _db.Categories.FindAsync(categoryForUpdate.Id);

            if (categoryFromDb is null)
                return false;

            categoryFromDb.Name = categoryForUpdate.Name;
            await _db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Deletes a category from the database.
        /// </summary>
        /// <param name="categoryForDelete">The category to delete.</param>
        /// <returns>True if the category was deleted, false otherwise.</returns>
        public async Task<bool> DeleteCategoryAsync(Category categoryForDelete)
        {
            var categoryFromDb = await _db.Categories.FindAsync(categoryForDelete.Id);

            if (categoryFromDb is null)
                return false;

            _db.Categories.Remove(categoryFromDb);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
