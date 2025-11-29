using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infa.Repositories
{
    public class AuthRepository: IAuthRepoository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
            => await _userManager.CheckPasswordAsync(user, password);

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
            => await _userManager.GetRolesAsync(user);

        public async Task<bool> RoleExistsAsync(string roleName)
            => await _roleManager.RoleExistsAsync(roleName);

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
            => await _roleManager.CreateAsync(new IdentityRole(roleName));

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName)
            => await _userManager.AddToRoleAsync(user, roleName);

        public IQueryable<ApplicationUser> GetAllUsers() => _userManager.Users;


    }
}
