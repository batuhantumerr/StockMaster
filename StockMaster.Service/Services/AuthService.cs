using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StockMaster.Core.DTOs;
using StockMaster.Core.Entities;
using StockMaster.Core.Repositories;
using StockMaster.Core.UnitOfWorks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockMaster.Service.Services
{
    public class AuthService
    {
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguratiçon _configuration; // appsettings'i okumak için

        public AuthService(IGenericRepository<AppUser> userRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // Login İşlemi
        public async Task<CustomResponseDto<TokenDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.Where(x => x.Email == loginDto.Email).FirstOrDefaultAsync();

            if (user == null || user.Password != loginDto.Password)
            {
                return CustomResponseDto<TokenDto>.Fail(404, "Email veya şifre hatalı");
            }

            var token = CreateToken(user);
            return CustomResponseDto<TokenDto>.Success(200, token);
        }

        // Kayıt İşlemi
        public async Task<CustomResponseDto<NoContentDto>> RegisterAsync(RegisterDto registerDto)
        {
            var isExist = await _userRepository.AnyAsync(x => x.Email == registerDto.Email);
            if (isExist) return CustomResponseDto<NoContentDto>.Fail(400, "Bu email zaten kayıtlı");

            var user = new AppUser
            {
                Email = registerDto.Email,
                Password = registerDto.Password, // Normalde Hashlenmeli!
                UserName = registerDto.UserName,
                Role = "Admin", // Şimdilik herkes Admin :)
                CreatedDate = DateTime.Now
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(204);
        }

        // Yardımcı Metod: Token Üretici
        private TokenDto CreateToken(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenOptions:SecurityKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var expiration = DateTime.Now.AddMinutes(int.Parse(_configuration["TokenOptions:AccessTokenExpiration"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["TokenOptions:Issuer"],
                audience: _configuration["TokenOptions:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            var handler = new JwtSecurityTokenHandler();

            return new TokenDto
            {
                AccessToken = handler.WriteToken(token),
                Expiration = expiration
            };
        }
    }
}