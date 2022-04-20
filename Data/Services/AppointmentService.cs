using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorShop.Data.DTOs;
using BlazorShop.Data.Models;
using BlazorShop.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Data.Services
{
    public class AppointmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProductService _productService;

        public AppointmentService(ApplicationDbContext db, ProtectedLocalStorage cartStorage,
            IHttpContextAccessor httpContextAccessor, ProductService productService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }

        // 1. Create appointment
        public async Task<int> CreateAppointmrntAsync(Appointment newAppointment, List<Product> shoppingCartProductList)
        {
            if (newAppointment is null)
                return 0;
            // Урок 12/2 (4) /////////////////////////////////////
            // Сохраняем информацию о заказе в базу для генерации и получения ID заказа

            _db.Appointments.Add(newAppointment);
            await _db.SaveChangesAsync();

            // Получаем ID пользователя
            // 1. Получаем объект типа ClaimsPrincipal
            var currentUser = _httpContextAccessor.HttpContext.User;

            // 2. Получаем объект идентификации юзера
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;

            // 3. Получаем доступ к объекту юзера
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // 4. Получаем ID покупателя
            var userId = claim?.Value;

            // Создаём переменную для хранения ID заказа (OrderId)
            int orderId = 0;

            // Создаём новый экземпляр класса Order
            OrderModel order = new();

            // Заполняем модель данными и сохраняем
            order.AppointmentId = newAppointment.Id;
            order.UserId = userId;
            order.CreatedAt = DateTime.Now;

            // Сохраняем результат в базу для генерации ID заказа
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Получаем ID заказа для формирования модели деталей
            orderId = order.Id;

            // Добавляем данные в модель
            foreach (var item in shoppingCartProductList)
            {
                // Объявляем модель деталей заказа OrderDetails
                OrderDetails details = new();

                details.OrderId = orderId;
                details.UserId = userId;

                details.ProductId = item.Id;
                details.Quantity = item.Quantity;

                order.OrderDetails.Add(details);
            }

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            return newAppointment.Id;
        }

        public async Task<Appointment> GetCurrentAppointmentAsync(int appointmentId)
        {
            return await _db.Appointments.FindAsync(appointmentId);
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _db.Appointments.ToListAsync();
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync(int pageIndex, int pageSize)
        {
            return await _db.Appointments.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<bool> ConfirmAppointmentAsync(int id)
        {
            var appFromDb = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id);

            if (appFromDb is null)
                return false;

            appFromDb.IsConfirmed = !appFromDb.IsConfirmed;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAppointmentAsync(Appointment appForDeleting)
        {
            if (appForDeleting is null)
                return false;

            _db.Appointments.Remove(appForDeleting);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            // 2. Получаем объект идентификации юзера
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;

            // 3. Получаем доступ к объекту юзера
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // 4. Получаем ID покупателя
            var userId = claim?.Value;

            return await _db.ApplicationUsers.FindAsync(userId);
        }

        public async Task<ProductsAndPrice> MakeTotalPriceAndGetAllProducts(List<int> listOfShoppingCart)
        {
            ProductsAndPrice resultData = new();

            if (listOfShoppingCart is not null)
            {
                for (int i = 0; i < listOfShoppingCart.Count; i++)
                {
                    var currentProduct = await _productService.GetSingleProductAsync(listOfShoppingCart[i]);

                    if (resultData.Products?.Exists(x => x.Id.Equals(currentProduct.Id)) == true)
                        resultData.Products.FirstOrDefault(x => x.Id == currentProduct.Id).Quantity++;
                    else
                    {
                        currentProduct.Quantity = 1;
                        resultData.Products.Add(currentProduct);
                    }
                }
                resultData.Products.ForEach(x => resultData.TotalPrice += x.Price * x.Quantity);
                return resultData;
            }
            return null;
        }
    }
}
