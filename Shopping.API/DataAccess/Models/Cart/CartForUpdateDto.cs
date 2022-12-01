using Shopping.API.DataAccess.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.API.DataAccess.Models
{
    public class CartForUpdateDto
    {
        [Required]
        public int Quantity { get; set; }

        [JsonIgnore]
        public double Price { get; set; }
    }
}
