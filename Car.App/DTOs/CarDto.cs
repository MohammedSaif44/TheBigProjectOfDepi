using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.DTOs
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public string Status { get; set; } = "Available";
        public string? ImageUrl { get; set; }
    }
    public class CreateCarDto
    {
        [Required]
        [RegularExpression(@"^(?!\d+$).+", ErrorMessage = "Make cannot be numbers only")]
        public string Make { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(?!\d+$).+", ErrorMessage = "Model cannot be numbers only")]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Range(1990, 2050, ErrorMessage = "Year must be between 1990 and 2050")]
        public int Year { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Price per day must be greater than 0")]
        public decimal PricePerDay { get; set; }

        [Required]
        public IFormFile? Image { get; set; }
    }
    public class UpdateCarDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^(?!\d+$).+", ErrorMessage = "Make cannot be numbers only")]
        public string Make { get; set; }

        [Required]
        [RegularExpression(@"^(?!\d+$).+", ErrorMessage = "Model cannot be numbers only")]
        public string Model { get; set; }

        [Range(1990, 2050, ErrorMessage = "Year must be between 1990 and 2050")]
        public int Year { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Price per day must be greater than 0")]
        public decimal PricePerDay { get; set; }

        [Required]
        [RegularExpression(@"^(Available|Rented)$", ErrorMessage = "Status must be 'Available' or 'Rented'")]
        public string Status { get; set; }
        public IFormFile? Image { get; set; }
        public string? OldImageUrl { get; set; }

    }
    public class DeleteDto
    {
        [Required]
        public int Id { get; set; }
    }
    public class FilterDto
    {
        [RegularExpression(@"^(?!\d+$).+", ErrorMessage = "Make cannot be numbers only")]
        public string? Make { get; set; }
        [RegularExpression(@"^(?!\d+$).+", ErrorMessage = "Model cannot be numbers only")]
        public string? Model { get; set; }

        [Range(1990, 2050)]
        public int? Year { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? MinPrice { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? MaxPrice { get; set; }
        [RegularExpression(@"^(Available|Rented)$", ErrorMessage = "Status must be 'Available' or 'Rented'")]
        public string? Status { get; set; }

    }
}
