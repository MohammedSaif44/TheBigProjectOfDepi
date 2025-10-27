using CarRental.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Interfaces
{
    public interface IAuthRepoository
    {
        Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);
        IQueryable<ApplicationUser> GetAllUsers();
    }
}
