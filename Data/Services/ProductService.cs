using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Data.Services
{
    // Урок 4 (4)
    public class ProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Product> GetSingleProductAsync(int productId)
        {
            // Урок 4 (5)
            var retProd = await _db.Products.Include(x => x.Category).Include(x => x.SpecialTag).FirstOrDefaultAsync(x => x.Id == productId);
            return retProd;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            // Урок 4 (5)
            return await _db.Products.Include(x => x.Category).Include(x => x.SpecialTag).ToListAsync();
        }

        // Метод получения списка категорий
        public async Task<List<Category>> GetCategoryListAsync()
        {
            // Урок 4 (5)
            return await _db.Categories.ToListAsync();
        }

        public async Task<List<SpecialTag>> GetSpecialTagsListAsync()
        {
            return await _db.SpecialTags.ToListAsync();
        }

        public async Task<bool> CreateProductAsync(Product newProduct)
        {
            if (newProduct is null)
                return false;

            await _db.Products.AddAsync(newProduct);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProductAsync(Product productForUpdate)
        {
            var productFromDb = await _db.Products.FindAsync(productForUpdate.Id);

            if (productFromDb is null)
                return false;

            // Урок 4 (6)
            // Проверяем, получена ли картинка
            if (productForUpdate.Image == null)
                productForUpdate.Image = productFromDb.Image;

            // Заменяем ручное обновление методом Entity Framework
            _db.Products.Update(productForUpdate);

            //productFromDb.Name = productForUpdate.Name;
            await _db.SaveChangesAsync();

            return true;
        }

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
