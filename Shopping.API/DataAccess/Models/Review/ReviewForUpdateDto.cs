﻿using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    /// <summary>
    /// A DTO for review to update
    /// </summary>
    public class ReviewForUpdateDto
    {
        /// <summary>
        /// Rating for product
        /// </summary>
        [Required]
        public int Rating { get; set; }

        /// <summary>
        /// Content of review
        /// </summary>
        [MaxLength(255)]
        public string? Content { get; set; }
    }
}
