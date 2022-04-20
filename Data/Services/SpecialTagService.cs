using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorShop.Data.Models;
using Microsoft.EntityFrameworkCore;

// Урок 1 (6)
namespace BlazorShop.Data.Services
{
    public class SpecialTagService
    {
        // Урок 1 (6.1) ---------------------------------
        private readonly ApplicationDbContext _db;

        public SpecialTagService(ApplicationDbContext db)
        {
            _db = db;
        }
        // ----------------------------------------------

        // Урок 1 (6.2) ---------------------------------
        public async Task<SpecialTag> GetSingleSpecialTagAsync(int specialTagId)
        {
            // Получаем категорию из базы данных
            // Возвращаем из метода найденную по ID категорию
            return await _db.SpecialTags.FindAsync(specialTagId);
        }
        // ----------------------------------------------

        // Урок 1 (6.2) ---------------------------------
        public async Task<List<SpecialTag>> GetAllSpecialTagsAsync()
        {
            // 2. Получаем все категории из базы данных
            // Возвращаем из метода в виде массива типа List
            return await _db.SpecialTags.ToListAsync();
        }
        // ----------------------------------------------

        public List<SpecialTag> GetPagedSpecialTagsAsync(int pageNumber, int pageSize, out int totalCount)
        {
            totalCount = _db.SpecialTags.Count();
            var pages = Task.Run(async () => await _db.SpecialTags.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()).Result;
            return pages;
        }

        // Урок 1 (6.2) ---------------------------------
        public async Task<bool> CreateSpecialTagAsync(SpecialTag newSpecialTag)
        {
            // Проверяем полученный параметр на null
            // И если он null, то возвращаем false (категория не создана)
            if (newSpecialTag is null)
                return false;

            // В противном случаи, передаём её в сущности Entity для добавления в базу данных
            await _db.SpecialTags.AddAsync(newSpecialTag);

            // Далее, сохраняем новую категорию в базу данных с помощью созданных сущностей Entity
            await _db.SaveChangesAsync();

            // Возвращаем true (категория создана успешно)
            return true;
        }
        // ----------------------------------------------

        // Урок 1 (6.2) ---------------------------------
        public async Task<bool> UpdateSpecialTagAsync(SpecialTag specialTagsForUpdate)
        {
            // Получаем редактируюмаю категорию из базы данных по ID переданной
            // Это нужно, чтобы Entity Framework знал, какую модель в базе данных и как обновлять
            var specialTagFromDb = await _db.SpecialTags.FindAsync(specialTagsForUpdate.Id);

            // Проверяем полученный параметр на null
            // И если он null, то возвращаем false (категория не обновлена)
            if (specialTagFromDb is null)
                return false;

            // В противном случаи, обновляем данные в модели из базы данных с помощью переданной модели
            // Так, как Entity Framework следит за обновлением полученной из базы модели
            // Нам остаётся обновить данные модели и сказать Entity Framework'у, чтобы он сохранил изменения в базе данных
            specialTagFromDb.Name = specialTagsForUpdate.Name;
            await _db.SaveChangesAsync();

            return true;
        }

        // Урок 1 (6.2) ---------------------------------
        public async Task<bool> DeleteSpecialTagAsync(SpecialTag specialTagsForDelete)
        {
            var specialTagFromDb = await _db.SpecialTags.FindAsync(specialTagsForDelete.Id);

            if (specialTagFromDb is null)
                return false;

            // Удаляем из сущьностей Entity найденную в базе категорию
            _db.SpecialTags.Remove(specialTagFromDb);
            await _db.SaveChangesAsync();

            // Возвращаем true (категория обновлена успешно)
            return true;
        }
        // ----------------------------------------------
    }
}
