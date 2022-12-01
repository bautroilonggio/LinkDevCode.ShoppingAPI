using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    public class OrderForUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string ShipName { get; set; }

        [Required]
        [MaxLength(150)]
        public string ShipAddress { get; set; }

        [Required]
        [MaxLength(10)]
        public string ShipPhone { get; set; }

        [Required]
        [MaxLength(100)]
        public string Status { get; set; }

        public OrderForUpdateDto(string shipName, string shipAddress, string shipPhone, string status)
        {
            ShipName = shipName;
            ShipAddress = shipAddress;
            ShipPhone = shipPhone;
            Status = status;
        }
    }
}
