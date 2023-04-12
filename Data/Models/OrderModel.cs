using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorShop.Data.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AppointmentId { get; set; }
        public int? PaymentDetailsId { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser Customer { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; }
        public List<OrderDetails> OrderDetails { get; set; } = new();

        [ForeignKey(nameof(PaymentDetailsId))]
        public PaymentDetails PaymentDetails { get; set; }
    }
}
