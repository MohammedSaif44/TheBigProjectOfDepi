using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Core.Entities
{
    public class NotificationPreference
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public bool EmailOnReservationCreated { get; set; } = true;
        public bool EmailOnReservationUpdated { get; set; } = true;
        public bool EmailOnPaymentSuccess { get; set; } = true;
        public bool EmailOnPaymentFailed { get; set; } = true;

        public ApplicationUser? User { get; set; }
    }

}
