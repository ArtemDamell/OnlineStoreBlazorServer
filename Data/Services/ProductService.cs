using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Data.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves a single product from the database based on the productId.
        /// </summary>
        /// <param name="productId">The Id of the product to be retrieved.</param>
        /// <returns>A Product object.</returns>
        public async Task<Product> GetSingleProductAsync(int productId)
        {
            var retProd = await _db.Products.Include(x => x.Category).Include(x => x.SpecialTag).FirstOrDefaultAsync(x => x.Id == productId);
            return retProd;
        }

        /// <summary>
        /// Asynchronously retrieves a list of all products from the database, including their associated category and special tag.
        /// </summary>
        public Task<List<Product>> GetAllProductsAsync() => _db.Products.Include(x => x.Category).Include(x => x.SpecialTag).ToListAsync();

        /// <summary>
        /// Asynchronously retrieves a list of categories from the database.
        /// </summary>
        public Task<List<Category>> GetCategoryListAsync() => _db.Categories.ToListAsync();

        /// <summary>
        /// Gets a list of SpecialTags asynchronously.
        /// </summary>
        public Task<List<SpecialTag>> GetSpecialTagsListAsync() => _db.SpecialTags.ToListAsync();

        /// <summary>
        /// Creates a new product in the database.
        /// </summary>
        /// <param name="newProduct">The product to be created.</param>
        /// <returns>True if the product was created successfully, false otherwise.</returns>
        public async Task<bool> CreateProductAsync(Product newProduct)
        {
            if (newProduct is null)
                return false;

            await _db.Products.AddAsync(newProduct);
            await _db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Updates a product in the database.
        /// </summary>
        /// <param name="productForUpdate">The product to be updated.</param>
        /// <returns>True if the product was successfully updated, false otherwise.</returns>
        public async Task<bool> UpdateProductAsync(Product productForUpdate)
        {
            var productFromDb = await _db.Products.FindAsync(productForUpdate.Id);

            if (productFromDb is null)
                return false;

            if (productForUpdate.Image == null)
                productForUpdate.Image = productFromDb.Image;

            _db.Products.Update(productForUpdate);
            await _db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Deletes a product from the database.
        /// </summary>
        /// <param name="productForDelete">The product to be deleted.</param>
        /// <returns>True if the product was successfully deleted, false otherwise.</returns>
        public async Task<bool> DeleteProductAsync(Product productForDelete)
        {
            var productFromDb = await _db.Products.FindAsync(productForDelete.Id);

            if (productFromDb is null)
                return false;

            _db.Products.Remove(productFromDb);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
