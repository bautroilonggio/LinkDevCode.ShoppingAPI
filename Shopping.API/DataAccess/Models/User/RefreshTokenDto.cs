using System.Text.Json.Serialization;

namespace Shopping.API.DataAccess.Models
{
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime RefreshTokenCreatedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime RefreshTokenExpries { get; set; }
    }
}
