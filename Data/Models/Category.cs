﻿using System.ComponentModel.DataAnnotations;

// Урок 1 (3)
namespace BlazorShop.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, Display(Name = "Category Name")]
        [MinLength(2), MaxLength(20)]
        public string Name { get; set; }
    }
}
