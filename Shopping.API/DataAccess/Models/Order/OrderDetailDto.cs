using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class OrderDetailDto
    {
        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
