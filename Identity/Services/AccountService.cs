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
using RapidPayTest.Application.DTOs.Settings;
using RapidPayTest.Application.Exceptions;
using RapidPayTest.Application.Interfaces.Services.Security;
using RapidPayTest.Application.Wrappers;
using RapidPayTest.Identity.Helpers;
using RapidPayTest.Identity.Models;
using RapidPayTest.Application.Interfaces.Repositories;
using RapidPayTest.Domain.Entities;
using RapidPayTest.Application.DTOs;
using AutoMapper;
using RapidPayTest.Application.DTOs.ViewModel;
using System.Linq.Expressions;
using FluentValidation;

namespace RapidPayTest.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepositoryAsync<User> _userRepo;
        private readonly IValidator<UserDto> _validator;
        private readonly ICryptographyProcessorService _cryptographyProcessorService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JWTSettings _jwtSettings;

        public AccountService(JWTSettings jwtSettings, IRepositoryAsync<User> userRepo, IValidator<UserDto> validator, ICryptographyProcessorService cryptographyProcessorService, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userRepo = userRepo;
            _validator = validator;
            _cryptographyProcessorService = cryptographyProcessorService;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtSettings = jwtSettings;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            //List<Expression<Func<User, object>>> includes = new List<Expression<Func<User, object>>>();
            //includes.Add(d => d.Role);

            if (!await _userRepo.Exists(x => x.Email == request.Email))
            {
                throw new ApiException($"User {request.Email} not found.");
            }
            var userData = await _userRepo.WhereAllAsync(x => x.Email.Equals(request.Email));
            var user = userData.LastOrDefault();

            if (user.IsDeleted == true)
            {
                throw new ApiException($"User {request.Email} is disabled.");
            }
            var PasswordsMatchResult = _cryptographyProcessorService.PasswordsMatch(request.Password, user.PasswordKey, user.Password);
            if (!PasswordsMatchResult)
            {
                throw new ApiException($"Invalid password");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWTToken(user);
            AuthenticationResponse response = new AuthenticationResponse();
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Expiration = DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes);
            response.ValidationContrato = user.ContractValidation;

            user.LastAccess = DateTime.Now;

            await _userRepo.UpdateAsync(user);
            return new Response<AuthenticationResponse>(response);
        }

        public async Task<Response<UserVm>> RegisterAsync(UserDto request, string origin)
        {
            var valResult = _validator.Validate(request);
            if (!valResult.IsValid) throw new ApiValidationException(valResult.Errors);

            var result = await _userRepo.Exists(x => x.Email.Equals(request.Email));

            if (result == true)
            {
                throw new KeyNotFoundException($"already exist ={request.Email}");
            }

            var usuario = new ApplicationUser
            {
                Email = request.Email,
                Name = request.Name,
                LastName = request.LastName,
                IdentificationNumber = request.SocialNumber,
                UserName = request.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var obj = _mapper.Map<User>(request);
            var password = _cryptographyProcessorService.GetPasswordAndSecurityKeyInfo(request.Password);
            obj.Password = password.HashedPassword;
            obj.PasswordKey = password.SecurityKey;
            obj.CreateAt = DateTime.UtcNow;
            obj.IsDeleted = false;
            obj.ContractValidation = true;
            obj.RoleID = request.RoleID;
            obj.Status = "A";

            return new Response<UserVm>(_mapper.Map<UserVm>(await _userRepo.AddAsync(obj)));
        }

        public async Task<PagedResponse<IList<UserVm>>> GetUsers(int pageNumber, int pageSize, string filter = null)
        {

            List<Expression<Func<User, bool>>> queryFilter = new List<Expression<Func<User, bool>>>();

            var list = await _userRepo.GetPagedList(pageNumber, pageSize, queryFilter);
            if (list == null || list.Data.Count == 0)
            {
                throw new KeyNotFoundException($"Users not found");
            }

            return new PagedResponse<IList<UserVm>>(_mapper.Map<IList<UserVm>>(list.Data), list.PageNumber, list.PageSize, list.TotalCount);
        }

        private async Task<JwtSecurityToken> GenerateJWTToken(User user)
        {
            //var userClaims = await _userManager.GetClaimsAsync(user);
            //var roles = await _userManager.GetRolesAsync(user);

            //var roleClaims = new List<Claim>();

            //for (int i = 0; i < roles.Count; i++)
            //{
            //    roleClaims.Add(new Claim("roles", roles[i]));
            //}

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Email", user.Email),
                new Claim("uid", user.ID.ToString()),
                new Claim("ip", ipAddress),
                new Claim("name", user.Name),
                new Claim("lastname", user.LastName),
                new Claim("IDUser",user.ID.ToString()),
                new Claim("IDRole",user.RoleID.ToString()),
                //new Claim("RolName",user.Role.RoleName.ToString()),
                //new Claim("RolId",user.IDRole),
             };

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
