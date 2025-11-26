using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string CarModel { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


public class CreateReservationDto
    {
        [Required(ErrorMessage = "CarId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CarId must be greater than 0")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }


    public class UpdateReservationDto
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression(@"^(Pending|Approved|Rejected|Cancelled)$",
            ErrorMessage = "Status must be Pending, Approved, Rejected, or Cancelled")]
        public string Status { get; set; }
    }
}
