using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorShop.Data.DTOs;
using BlazorShop.Data.Models;
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

        /// <summary>
        /// Creates an appointment and order with the given parameters.
        /// </summary>
        /// <param name="newAppointment">The new appointment to be created.</param>
        /// <param name="shoppingCartProductList">The list of products associated with the order.</param>
        /// <returns>The ID of the created appointment.</returns>
        public async Task<int> CreateAppointmrntAsync(Appointment newAppointment, List<Product> shoppingCartProductList)
        {
            if (newAppointment is null)
                return 0;

            _db.Appointments.Add(newAppointment);
            await _db.SaveChangesAsync();

            var currentUser = _httpContextAccessor.HttpContext.User;
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userId = claim?.Value;

            int orderId = 0;
            OrderModel order = new();

            order.AppointmentId = newAppointment.Id;
            order.UserId = userId;
            order.CreatedAt = DateTime.Now;
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            orderId = order.Id;

            foreach (var item in shoppingCartProductList)
            {
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

        /// <summary>
        /// Retrieves the current appointment from the database asynchronously.
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment to retrieve.</param>
        /// <returns>The current appointment.</returns>
        public async Task<Appointment> GetCurrentAppointmentAsync(int appointmentId) => await _db.Appointments.FindAsync(appointmentId);

        /// <summary>
        /// Retrieves a list of all Appointments from the database asynchronously.
        /// </summary>
        public Task<List<Appointment>> GetAllAppointmentsAsync() => _db.Appointments.ToListAsync();

        /// <summary>
        /// Gets a list of Appointments from the database asynchronously.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A list of Appointments.</returns>
        public Task<List<Appointment>> GetAllAppointmentsAsync(int pageIndex, int pageSize) => _db.Appointments.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        /// <summary>
        /// Confirms an appointment with the given id.
        /// </summary>
        /// <param name="id">The id of the appointment to be confirmed.</param>
        /// <returns>True if the appointment was successfully confirmed, false otherwise.</returns>
        public async Task<bool> ConfirmAppointmentAsync(int id)
        {
            var appFromDb = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id);

            if (appFromDb is null)
                return false;

            appFromDb.IsConfirmed = !appFromDb.IsConfirmed;
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Asynchronously deletes an appointment from the database.
        /// </summary>
        /// <param name="appForDeleting">The appointment to be deleted.</param>
        /// <returns>True if the appointment was successfully deleted, false otherwise.</returns>
        public async Task<bool> DeleteAppointmentAsync(Appointment appForDeleting)
        {
            if (appForDeleting is null)
                return false;

            _db.Appointments.Remove(appForDeleting);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves the current user from the database.
        /// </summary>
        /// <returns>The current user.</returns>
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim?.Value;

            return await _db.ApplicationUsers.FindAsync(userId);
        }

        /// <summary>
        /// Calculates the total price of a list of products and returns all the products in the list.
        /// </summary>
        /// <param name="listOfShoppingCart">List of products to calculate the total price.</param>
        /// <returns>A ProductsAndPrice object containing the total price and all the products in the list.</returns>
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
