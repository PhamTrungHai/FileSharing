using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaritasaT.Models;
using SaritasaT.Securities;
using SaritasaT.Services;
using System.Text;

namespace SaritasaT.Extensions
{
    public static class ServicesRegistor
    {
        public static void RegisterServices(this WebApplicationBuilder builder) 
        {
            builder.Services.AddCors();
            builder.Services.AddControllers();
            //Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Environment.GetEnvironmentVariable("ASPNETCORE_ISSUER"),
                    ValidAudience = Environment.GetEnvironmentVariable("ASPNETCORE_AUDIENCE"),
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ASPNETCORE_JWTKEY"))),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                };
            });
            builder.Services.AddAuthorization();

            // Application database context
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // DI for UserService
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStorageService, StorageService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddSingleton<IBlobService, BlobService>();
            builder.Services.AddSingleton<IDataProtection>();
        }
    }
}
