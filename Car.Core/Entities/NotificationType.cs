using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Core.Entities
{
    public enum NotificationType
    {
        ReservationCreated = 1,
        ReservationUpdated = 2,
        ReservationCancelled = 3,
        PaymentSuccess = 4,
        PaymentFailed = 5
    }

}
