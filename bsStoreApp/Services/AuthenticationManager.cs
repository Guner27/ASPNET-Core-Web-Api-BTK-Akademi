using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class AuthenticationManager : IAuthenticationService
    {
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private User? _user;

        public AuthenticationManager(
            ILoggerService logger,
            IMapper mapper,
            UserManager<User> userManager,
            IConfiguration config)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _config = config;
        }

        public async Task<string> CreateToken()
        {
            var signinCredentials = GetSigninCredentials(); //Kimlik bilgilerini al
            var claims = await GetClaims();     //Rol bilgilerini al
            var TokenOptions = GenerateTokenOptions(signinCredentials, claims); //Token oluşturma ayarlarını yap
            return new JwtSecurityTokenHandler().WriteToken(TokenOptions);      //Token oluştur.
        }


        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var user = _mapper.Map<User>(userForRegistrationDto);

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);

            if (result.Succeeded)
                await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);
            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthDto)
        {
            _user = await _userManager.FindByNameAsync(userForAuthDto.UserName);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuthDto.Password));

            if (!result)
            {
                _logger.LogWarning($"{nameof(ValidateUser)} : Authentication failed. Wrong username or password.");
            }
            return result;
        }


        private SigningCredentials GetSigninCredentials()
        {
            var jwtSetting = _config.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSetting["secretKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            //Rol listesi
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var jwtSetting = _config.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSetting["validIssuer"],
                audience: jwtSetting["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSetting["expires"])),
                signingCredentials: signinCredentials);

            return tokenOptions;
        }
    }
}
