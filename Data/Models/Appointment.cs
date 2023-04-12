using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorShop.Data.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        [Required]
        [MinLength(2), MaxLength(30)]
        public string CustomerName { get; set; }
        [Phone]
        public string CustomerPhone { get; set; }
        [EmailAddress]
        public string CustomerEmail { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
