using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorShop.Extensions
{
    public static class SessionExtension
    {
        public delegate void NotifyDelegate(int count);
        public static event NotifyDelegate NotyfyChanges;

        /// <summary>
        /// Sets the given value in the session with the given key and notifies the changes.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task SetForSession<T>(this ProtectedLocalStorage session, string key, T value)
        {
            await session.SetAsync(key, JsonSerializer.Serialize(value));
            var list = await session.GetFromSession<List<int>>(key);
            NotyfyChanges?.Invoke(list.Count);
        }

        /// <summary>
        /// Retrieves an object from the ProtectedLocalStorage session.
        /// </summary>
        /// <param name="session">The ProtectedLocalStorage session.</param>
        /// <param name="key">The key of the object to retrieve.</param>
        /// <returns>The object retrieved from the session.</returns>
        public static async Task<T> GetFromSession<T>(this ProtectedLocalStorage session, string key)
        {
            var value = await session.GetAsync<string>(key);
            return value.Value == null ? default : JsonSerializer.Deserialize<T>(value.Value);
        }

        /// <summary>
        /// Deletes all data from the specified session with the given key.
        /// </summary>
        public static async Task DeleteAllFromSession(this ProtectedLocalStorage session, string key) => await session.DeleteAsync(key);
    }
}
