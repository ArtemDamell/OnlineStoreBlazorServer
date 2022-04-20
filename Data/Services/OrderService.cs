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

        public async Task<OrderModel> SavePaymentDetailsAsync(PayPalResponse detailsToSave)
        {
            // 1. Конвертируем ID заказа обратно в int
            int appointmentId;
            var isSuccess = Int32.TryParse(detailsToSave.transactions[0].description, out appointmentId);

            // 2. Проверяем на null
            if (!isSuccess)
                return null;

            // 3. Находим текущий заказ по ID
            var orderFromDb = await _db.Orders
                                        .Include(x => x.Appointment)
                                        .Include(x => x.Customer)
                                        .Include(x => x.OrderDetails)
                                        .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
            // 4. Проверяем на null
            if (orderFromDb is null)
                return null;

            // 5. Создаём новый объект информации об оплате
            PaymentDetails paymentDetails = new();

            paymentDetails.PayPalPaymentId = detailsToSave.id;
            paymentDetails.PaymentMethod = detailsToSave.payer.payment_method;
            paymentDetails.PaymentPrice = double.Parse(detailsToSave.transactions[0].amount.total, CultureInfo.InvariantCulture);
            paymentDetails.PaymentStatus = "Success";

            await _db.PaymentDetails.AddAsync(paymentDetails);
            await _db.SaveChangesAsync();

            // 6. На этом месте у нас есть уже ID деталей оплаты, теперь связываем детали заказа и оплаты
            orderFromDb.PaymentDetailsId = paymentDetails.Id;
            await _db.SaveChangesAsync();

            return orderFromDb;
        }

        public async Task<OrderModel> GetSingeOrderByIdAsync(int appointmentId)
        {
            return await _db.Orders.Include(x => x.Customer)
                                   .Include(x => x.OrderDetails).ThenInclude(x => x.Product)
                                   .Include(x => x.PaymentDetails)
                                   .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
        }
    }
}
