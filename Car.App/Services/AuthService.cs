using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Services
{
    public class AuthService
    {
        private readonly IAuthRepoository _repo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepoository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<object> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _repo.RegisterAsync(user, model.Password);
            if (!result.Succeeded)
                return new { Success = false, Errors = result.Errors };

            if (!await _repo.RoleExistsAsync("Customer"))
                await _repo.CreateRoleAsync("Customer");

            await _repo.AddToRoleAsync(user, "Customer");

            return new { Success = true, Message = "User registered successfully" };
        }

        public async Task<object> LoginAsync(LoginDto model)
        {
            var user = await _repo.GetUserByEmailAsync(model.Email);
            if (user == null || !await _repo.CheckPasswordAsync(user, model.Password))
                return new { Success = false, Message = "Invalid email or password" };

            var roles = await _repo.GetUserRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return new { Success = true, Token = token };
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)

            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                expires: DateTime.Now.AddHours(2),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRoleAsync(string roleName)
        {
            if (await _repo.RoleExistsAsync(roleName))
                return "Role already exists";

            var result = await _repo.CreateRoleAsync(roleName);
            return result.Succeeded ? "Role created successfully" : "Failed to create role";
        }

        public async Task<string> AssignRoleAsync(AssignRoleDto model)
        {
            var user = await _repo.GetUserByEmailAsync(model.Email);
            if (user == null) return "User not found";

            var result = await _repo.AddToRoleAsync(user, model.RoleName);
            return result.Succeeded ? $"Role {model.RoleName} assigned to {model.Email}" : "Failed to assign role";
        }

        public async Task<IEnumerable<object>> GetAllUsersAsync()
        {
            var users = _repo.GetAllUsers().ToList();
            var list = new List<object>();

            foreach (var user in users)
            {
                var roles = await _repo.GetUserRolesAsync(user);
                list.Add(new { user.FullName, user.Email, Roles = roles });
            }

            return list;
        }
    }
}
