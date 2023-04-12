using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlazorShop.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone")]
        [Phone]
        [MaxLength(10)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }
    }
}
