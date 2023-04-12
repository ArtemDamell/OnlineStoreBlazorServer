using BlazorShop.Data.Models;
using BlazorShop.Merchant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace BlazorShop.Data.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _db;
        public OrderService(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Saves the payment details from PayPal response to the database.
        /// </summary>
        /// <param name="detailsToSave">The PayPal response containing the payment details.</param>
        /// <returns>The order model with the payment details.</returns>
        public async Task<OrderModel> SavePaymentDetailsAsync(PayPalResponse detailsToSave)
        {
            int appointmentId;
            var isSuccess = Int32.TryParse(detailsToSave.transactions[0].description, out appointmentId);

            if (!isSuccess)
                return null;

            var orderFromDb = await _db.Orders
                                        .Include(x => x.Appointment)
                                        .Include(x => x.Customer)
                                        .Include(x => x.OrderDetails)
                                        .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);

            if (orderFromDb is null)
                return null;

            PaymentDetails paymentDetails = new();

            paymentDetails.PayPalPaymentId = detailsToSave.id;
            paymentDetails.PaymentMethod = detailsToSave.payer.payment_method;
            paymentDetails.PaymentPrice = double.Parse(detailsToSave.transactions[0].amount.total, CultureInfo.InvariantCulture);
            paymentDetails.PaymentStatus = "Success";

            await _db.PaymentDetails.AddAsync(paymentDetails);
            await _db.SaveChangesAsync();

            orderFromDb.PaymentDetailsId = paymentDetails.Id;
            await _db.SaveChangesAsync();

            return orderFromDb;
        }

        /// <summary>
        /// Retrieves a single order by its appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID of the order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the order model.</returns>
        public Task<OrderModel> GetSingeOrderByIdAsync(int appointmentId)
        {
            return _db.Orders.Include(x => x.Customer)
                                   .Include(x => x.OrderDetails).ThenInclude(x => x.Product)
                                   .Include(x => x.PaymentDetails)
                                   .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
        }
    }
}
