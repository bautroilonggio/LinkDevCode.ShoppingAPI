using Shopping.API.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.API.DataAccess.Models
{
    public class OrderDto
    {
        public int Id { get; set; }

        public DateOnly OrderDate { get; set; }

        public string? ShipName { get; set; }

        public string? ShipAddress { get; set; }

        public string? ShipPhone { get; set; }

        public string? Status { get; set; }

        public double SumOfPrice { get; set; }

        public int NumberOfOrderDetails 
        {
            get
            {
                return OrderDetails.Count;
            }
        }

        public ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
}
