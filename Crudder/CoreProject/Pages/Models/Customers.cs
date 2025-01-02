
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using Data;

namespace Models
{
    public class Customers
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}