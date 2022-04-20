using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorShop.Data.Models
{
    public class PaymentDetails
    {
        public int Id { get; set; }
        public string PayPalPaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public double PaymentPrice { get; set; }
        public string PaymentStatus { get; set; }
    }
}
