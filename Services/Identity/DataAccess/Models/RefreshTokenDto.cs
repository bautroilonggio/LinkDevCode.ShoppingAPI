using System.Text.Json.Serialization;

namespace Identity.DataAccess.Models
{
    /// <summary>
    /// A DTO for a refresh token of an account
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// String of refresh token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Creation time of refresh token
        /// </summary>
        [JsonIgnore]
        public DateTime RefreshTokenCreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Expiration time of refresh token
        /// </summary>
        [JsonIgnore]
        public DateTime RefreshTokenExpries { get; set; }
    }
}
