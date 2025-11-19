using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using CarRental.Infa.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infa.Repositories
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly ApplicationDbContext _ctx;

        public EmailTemplateRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<EmailTemplate?> GetByKeyAsync(string key)
        {
            return await _ctx.EmailTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TemplateKey == key);
        }
    }
}
