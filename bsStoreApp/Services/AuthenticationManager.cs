using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="populateExp">Bu ifade True ise RefreshToken süre uzatması yap. False ise süreye dokunma</param>
        /// <returns></returns>
        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var signinCredentials = GetSigninCredentials(); //Kimlik bilgilerini al
            var claims = await GetClaims();     //Rol bilgilerini al
            var TokenOptions = GenerateTokenOptions(signinCredentials, claims); //Token oluşturma ayarlarını yap

            var RefreshToken = GenerateRefreshToken();
            _user.RefreshToken = RefreshToken;

            if (populateExp)
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(_user);


            var accessToken = new JwtSecurityTokenHandler().WriteToken(TokenOptions);      //Token oluştur.

            return new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = RefreshToken,
            };
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPricipalFromExpiredToken(string token)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];

            //Token Doğrulama, (Bunu gerçekten ben mi ürettüm)
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,  //Key'i kim ürettiyse onu doğrula
                ValidateAudience = true,    //Geçerli bir alıcı mı
                ValidateLifetime = true,    //Süresi var mı
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            //Doğrulama işleminden kullanıcı bilgilerini al.
            var pricipal = tokenHandler.ValidateToken(token,tokenValidationParameters, out securityToken);

            //Doğrulama metotu çalıştıktan sonra eğer bu securityToken oluşmuşsa validate işlemi gerçekleşiş oluyor. Ve kullanıcı bilgilerini dönebilirim.
            var jwtSecurityToken =securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)) 
            {
                throw new SecurityTokenException("Invalid Token");            
            }

            return pricipal;
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPricipalFromExpiredToken(tokenDto.AccessToken);

            //Veritabanında ilgili kullanıcı var mı yok mu?
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null ||
                user.RefreshToken != tokenDto.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new RefreshTokenBadRequestException();

            _user = user;
            return await CreateToken(populateExp: false);
        }
    }
}
