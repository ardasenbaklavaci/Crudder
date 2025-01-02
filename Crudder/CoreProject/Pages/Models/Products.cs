
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using Data;

namespace Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}