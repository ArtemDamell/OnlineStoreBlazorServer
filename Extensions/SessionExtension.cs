using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorShop.Data.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlazorShop.Extensions
{
    public static class SessionExtension
    {
        // Создаём делегат и событие изменения корзины
        public delegate void NotifyDelegate(int count);
        public static event NotifyDelegate NotyfyChanges;

        // Добавляем динамически типизированный SET метод
        public static async Task SetForSession<T>(this ProtectedLocalStorage session, string key, T value)
        {
            await session.SetAsync(key, JsonSerializer.Serialize(value));
            var list = await session.GetFromSession<List<int>>(key);
            NotyfyChanges?.Invoke(list.Count);
        }

        // Добавляем динамически типизированный GET метод
        public static async Task<T> GetFromSession<T>(this ProtectedLocalStorage session, string key)
        {
            var value = await session.GetAsync<string>(key);
            return value.Value == null ? default : JsonSerializer.Deserialize<T>(value.Value);
        }

        // Добавляем динамически типизированный DELET метод
        public static async Task DeleteAllFromSession(this ProtectedLocalStorage session, string key)
        {
            await session.DeleteAsync(key);
        }
    }
}
