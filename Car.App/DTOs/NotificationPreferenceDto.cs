using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.DTOs
{
    public class NotificationPreferenceDto
    {
        public bool EmailOnReservationCreated { get; set; }
        public bool EmailOnReservationUpdated { get; set; }
        public bool EmailOnPaymentSuccess { get; set; }
        public bool EmailOnPaymentFailed { get; set; }
    }

}
