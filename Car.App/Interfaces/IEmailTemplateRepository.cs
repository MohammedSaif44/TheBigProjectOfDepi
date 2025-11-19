using CarRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Interfaces
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate?> GetByKeyAsync(string key);
    }

}
