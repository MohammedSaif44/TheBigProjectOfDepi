using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [RegularExpression(@"^[a-zA-Zأ-ي\s]+$",ErrorMessage = "Full name must contain only letters and spaces")]
        public string FullName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^(?!\d+$).+", ErrorMessage = "Email cannot be numbers only")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }

}
