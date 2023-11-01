using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Text;
using RapidPayTest.Application.DTOs.Settings;
using RapidPayTest.Application.Interfaces.Services.Security;
using RapidPayTest.Application.Wrappers;
using RapidPayTest.Identity.Contexts;
using RapidPayTest.Identity.Models;
using RapidPayTest.Identity.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace RapidPayTest.Identity
{
    public static class ServiceExtensions
    {
        public static void AddIdentityInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("IdentityConnection"),
                b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            services.AddSingleton(JWTSettings => configuration.GetSection("JWTSettings").Get<JWTSettings>());

            #region Services
            services.AddScoped<ICryptographyProcessorService, CryptographyProcessorService>();
            services.AddScoped<IAccountService, AccountService>();
            #endregion

            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            services.AddScoped<IAccountService, AccountService>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };



                o.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("Unauthorize"));
                        return context.Response.WriteAsync(result);

                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("Unauthorize, without permitions"));
                        return context.Response.WriteAsync(result);
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy =>
                {
                    policy.RequireClaim("IDRole", "1"); // Verifica que el claim "IDRole" tenga el valor "1"
                });
            });



        }
    }
}
