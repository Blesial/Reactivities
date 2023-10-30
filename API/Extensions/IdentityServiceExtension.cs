using System.Text;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,
         IConfiguration config)
        {
            services
            .AddIdentityCore<AppUser>(opt =>
            {
                // the password has complex requirements in default
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;

            })
            // nos permite hacer un querie de nuestros users en ef store o DataBase!
            .AddEntityFrameworkStores<DataContext>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        // validamos si el token fue firmado por el api server y le damos el key.
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        // we dont going to validate this , we keep it simple
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            // addscoped -> el token service sera ligado al http request.
            // cuando el http request se recibe, vamos a el accountcontroller a solicitar el token, porqe el usuario esta intentando logearse
            services.AddScoped<TokenService>();

            return services;
        }
    }
}