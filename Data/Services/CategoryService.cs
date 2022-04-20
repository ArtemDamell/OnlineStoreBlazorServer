using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorShop.Data.Models;
using Microsoft.EntityFrameworkCore;

// Урок 1 (6)
namespace BlazorShop.Data.Services
{
    public class CategoryService
    {
        // Урок 1 (6.1) ---------------------------------
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }
        // ----------------------------------------------

        // Урок 1 (6.2) ---------------------------------
        public async Task<Category> GetSingleCategoryAsync(int categoryId)
        {
            // Получаем категорию из базы данных
            // Возвращаем из метода найденную по ID категорию
            return await _db.Categories.FindAsync(categoryId);
        }
        // ----------------------------------------------

        // Урок 1 (6.2) ---------------------------------
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            // 2. Получаем все категории из базы данных
            // Возвращаем из метода в виде массива типа List
            return await _db.Categories.ToListAsync();
        }
        // ----------------------------------------------
        public List<Category> GetPagedCategoriesAsync(int pageNumber, int pageSize, out int totalCount)
        {
            totalCount = _db.Categories.Count();
            var pages = Task.Run(async () => await _db.Categories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()).Result;
            return pages;
        }
        // Pagination lesson (3)

        // Урок 1 (6.2) ---------------------------------
        public async Task<bool> CreateCategoryAsync(Category newCategory)
        {
            // Проверяем полученный параметр на null
            // И если он null, то возвращаем false (категория не создана)
            if (newCategory is null)
                return false;

            // В противном случаи, передаём её в сущности Entity для добавления в базу данных
            await _db.Categories.AddAsync(newCategory);

            // Далее, сохраняем новую категорию в базу данных с помощью созданных сущностей Entity
            await _db.SaveChangesAsync();

            // Возвращаем true (категория создана успешно)
            return true;
        }
        // ----------------------------------------------

        // Урок 1 (6.2) ---------------------------------
        public async Task<bool> UpdateCategoryAsync(Category categoryForUpdate)
        {
            // Получаем редактируюмаю категорию из базы данных по ID переданной
            // Это нужно, чтобы Entity Framework знал, какую модель в базе данных и как обновлять
            var categoryFromDb = await _db.Categories.FindAsync(categoryForUpdate.Id);

            // Проверяем полученный параметр на null
            // И если он null, то возвращаем false (категория не обновлена)
            if (categoryFromDb is null)
                return false;

            // В противном случаи, обновляем данные в модели из базы данных с помощью переданной модели
            // Так, как Entity Framework следит за обновлением полученной из базы модели
            // Нам остаётся обновить данные модели и сказать Entity Framework'у, чтобы он сохранил изменения в базе данных
            categoryFromDb.Name = categoryForUpdate.Name;
            await _db.SaveChangesAsync();

            return true;
        }

        // Урок 1 (6.2) ---------------------------------
        public async Task<bool> DeleteCategoryAsync(Category categoryForDelete)
        {
            var categoryFromDb = await _db.Categories.FindAsync(categoryForDelete.Id);

            if (categoryFromDb is null)
                return false;

            // Удаляем из сущьностей Entity найденную в базе категорию
            _db.Categories.Remove(categoryFromDb);
            await _db.SaveChangesAsync();

            // Возвращаем true (категория обновлена успешно)
            return true;
        }
        // ----------------------------------------------
    }
}
