using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Core.Entities
{
    public class EmailTemplate
    {
        public int Id { get; set; }

        public string TemplateKey { get; set; } = string.Empty;  // Example: "ReservationCreated"

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty; // HTML template

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
