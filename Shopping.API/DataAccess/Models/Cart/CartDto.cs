using Shopping.API.DataAccess.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class CartDto
    {
        public int Id { get; set; }

        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string? UserName { get; set; }
    }
}
