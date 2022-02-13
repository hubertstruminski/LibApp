using LibApp.Data;
using LibApp.Dtos;
using LibApp.Exceptions;
using LibApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibApp.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto registerDto);

        string GenerateJWT(LoginUserDto loginDto);
    }

    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Customer> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(ApplicationDbContext context, 
            IPasswordHasher<Customer> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterUserDto registerDto)
        {
            var newUser = new Customer
            {
                Email = registerDto.Email,
                RoleId = registerDto.RoleId,
                Name = registerDto.Name,
                Birthdate = registerDto.Birthdate,
                MembershipTypeId = registerDto.MembershipTypeId,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, registerDto.Password);
            newUser.PasswordHash = hashedPassword;

            _context.Customers.Add(newUser);
            _context.SaveChanges();
        }

        public string GenerateJWT(LoginUserDto loginDto)
        {
            var user = _context.Customers
                .Include(c => c.Role)
                .FirstOrDefault(c => c.Email == loginDto.Email);

            if(user == null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.Name),
            };

            if(user.Birthdate != null)
            {
                claims.Add(new Claim("Birthdate", user.Birthdate.Value.ToString("yyyy-MM-dd")));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
