using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RappidPayTest.Application.DTOs.Settings;
using RappidPayTest.Application.Exceptions;
using RappidPayTest.Application.Interfaces.Services.Security;
using RappidPayTest.Application.Wrappers;
using RappidPayTest.Identity.Helpers;
using RappidPayTest.Identity.Models;

namespace RappidPayTest.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, JWTSettings jwtSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {

            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario == null)
            {
                throw new ApiException($"Email ${request.Email} not found.");
            }

            var result = await _signInManager.PasswordSignInAsync(usuario.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ApiException($"invalid user or password");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWTToken(usuario);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = usuario.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = usuario.Email;
            response.UserName = usuario.UserName;

            var rolesList = await _userManager.GetRolesAsync(usuario).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = usuario.EmailConfirmed;

            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            return new Response<AuthenticationResponse>(response, $"Authenticate {usuario.UserName}");
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var usuarioConElMismoUserName = await _userManager.FindByNameAsync(request.UserName);
            if (usuarioConElMismoUserName != null)
            {
                throw new ApiException($"This user= {request.UserName} already exists.");
            }
            var usuario = new ApplicationUser
            {
                Email = request.Email,
                Name = request.Name,
                LastName = request.LastName,
                IdentificationNumber = request.IdentificationNumber,
                UserName = request.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var usuarioConElMismoCorreo = await _userManager.FindByEmailAsync(request.Email);
            if (usuarioConElMismoCorreo != null)
            {
                throw new ApiException($"This email= {request.Email} already exists.");
            }
            else
            {
                var result = await _userManager.CreateAsync(usuario, request.Password);
                if (result.Succeeded)
                {
                    //await _userManager.AddToRoleAsync(usuario, Roles.Basic.ToString());
                    return new Response<string>(usuario.Id, message: $"Succesfully register {request.UserName}");
                }
                else
                {
                    throw new ApiException($"{result.Errors.ToString()}.");
                }
            }
        }

        private async Task<JwtSecurityToken> GenerateJWTToken(ApplicationUser usuario)
        {
            var userClaims = await _userManager.GetClaimsAsync(usuario);
            var roles = await _userManager.GetRolesAsync(usuario);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Email", usuario.Email),
                new Claim("uid", usuario.Id),
                new Claim("ip", ipAddress),
                new Claim("Name", usuario.Name),
                new Claim("LastName", usuario.LastName),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
