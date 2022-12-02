using Shopping.API.DataAccess.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.API.DataAccess.Models
{
    public class OrderForCreateDto
    {
        [JsonIgnore]
        public DateOnly OrderDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string ShipName { get; set; }

        [Required]
        [MaxLength(150)]
        public string ShipAddress { get; set; }

        [Required]
        [MaxLength(10)]
        public string ShipPhone { get; set; }
        
        [JsonIgnore]        
        public string Status { get; set; }

        [JsonIgnore]
        public double TotalPayment { get; set; }

        public ICollection<CartForCreateOrderDto> Carts { get; set; } = new List<CartForCreateOrderDto>();

        [JsonIgnore]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public OrderForCreateDto(string shipName, string shipAddress, string shipPhone)
        {
            OrderDate = DateOnly.Parse(DateTime.Now.ToShortDateString());
            ShipName = shipName;
            ShipAddress = shipAddress;
            ShipPhone = shipPhone;
            Status = "Dang chuan bi hang";
        }
    }
}
