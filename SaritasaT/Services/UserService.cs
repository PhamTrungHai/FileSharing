using SaritasaT.Helpers;
using SaritasaT.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SaritasaT.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        Task<User?> Authenticate(string email, string password);
        Task<User> GetById(int id);
        Task<User> Create(User model);
        string GenerateUserToken(User user);
        Task<User> Update(int id, User model);
        void Delete(int id);
    }
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(
            AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }
        public async Task<User?> Authenticate(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (BCrypt.Net.BCrypt.Verify(password,user.Password))
            {
                return user;
            }

            // on auth fail: null is returned because user is not found
            // on auth success: user object is returned
            return null;
        }

        public async Task<User> GetById(int id)
        {
            return await GetUser(id);
        }

        public async Task<User> Create(User model)
        {
            // validate
            if (await UserExists(model.Email))
                throw new AppException("User with the email '" + model.Email + "' already exists");

            // hash password
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // create instance user and storage
            User user = new()
            {
                Email = model.Email,
                Name = model.Name,
                Password = model.Password,
            };
            Storage storage = new ()
            {
                OwnerID=model.Id,
                User=model,
            };
            user.Storage=storage;
            // save 
            _context.Users.Add(user);
            _context.Storages.Add(storage);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(int id, User model)
        {
            var user = await GetUser(id);

            // validate
            if (await UserExists(model.Email))
                throw new AppException("User with the email '" + model.Email + "' already exists");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // copy model to user and save
            user = model;
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
            return user;
        }

        public async void Delete(int id)
        {
            var user = await GetUser(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }


        // helper methods
        public string GenerateUserToken(User user)
        {
            var issuer = Environment.GetEnvironmentVariable("ASPNETCORE_ISSUER");
            var audience = Environment.GetEnvironmentVariable("ASPNETCORE_AUDIENCE");
            var key = Encoding.ASCII.GetBytes
            (Environment.GetEnvironmentVariable("ASPNETCORE_JWTKEY"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddHours(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token).ToString();
            return stringToken;
        }

        private async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        private async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
