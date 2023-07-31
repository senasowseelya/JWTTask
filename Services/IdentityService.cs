using Azure;
using JWTDemo.DataContext;
using JWTDemo.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWTDemo.Services
{
	public class IdentityService
	{
		private readonly DemoTokenContext _context;
		private readonly IConfigurationRoot _config;

		public IdentityService(DemoTokenContext context)
		{
			_context = context;
			_config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
		}

		public ResponseModel<TokenModel> Login(LoginModel login)
		{
			ResponseModel<TokenModel> responseModel =  new ResponseModel<TokenModel>();
			//string hashedPassword = MD5.HashData(Encoding.UTF8.GetBytes(login.Password)).ToString();
			UsersMaster user = _context.UsersMasters.FirstOrDefault(user => user.UserName == login.UserName && user.Password == login.Password);
			if (user == null)
			{
				return new ResponseModel<TokenModel>
				{
					IsSuccess = false,
					Message = "Invalid User"
				};
			}
			AuthenticationResult result = Authenticate(user);
			if(result!=null  && result.Success)
			{
				responseModel.IsSuccess = true;
				responseModel.Data = new TokenModel { RefreshToken = result.RefreshToken, Token = result.Token };
			}
			else
			{
				responseModel.Message = "Something went wrong!";
				responseModel.IsSuccess = false;
			}
			return responseModel;
		}

		public List<RolesMaster> GetRoles(int UserId)
		{
			List<RolesMaster> roles = (from UR in _context.UserRoles
									   join UM in _context.UsersMasters
									   on UR.UserId equals UM.UserId
									   join RM in _context.RolesMasters
									   on UR.RoleId equals RM.RoleId
									   select RM
											 ).ToList();
			return roles;
		}

		public AuthenticationResult Authenticate(UsersMaster user)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				var key = _config.GetSection("ServiceConfiguration").GetSection("JwtSettings").GetSection("Secret").Value;
				ClaimsIdentity subject = new ClaimsIdentity();
				var claim = new List<Claim>
				{
					new Claim("UserId", user.UserId.ToString()),
					new Claim("UserName", user.UserName),
					new Claim("Email", user.Email),
					new Claim("FirstName", user.FirstName),
					new Claim("LastName", user.LastName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};
				subject.AddClaims(claim);
				foreach (var role in GetRoles(user.UserId))
				{
					subject.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
				}

				SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
				{
					Subject = subject,
					Issuer = "Sena.org",
					Expires = DateTime.Now.Add(TimeSpan.Parse(_config.GetSection("ServiceConfiguration").GetSection("JwtSettings").GetSection("TokenLifetime").Value)),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
				};
				SecurityToken token = tokenHandler.CreateToken(descriptor);
				AuthenticationResult result = new AuthenticationResult
				{
					Success = true,
					Token = tokenHandler.WriteToken(token)
				};
				RefreshToken refreshToken = new RefreshToken
				{
					RefreshTokenId = user.UserId + DateTime.Now.Millisecond,
					RefreshToken1 = Guid.NewGuid().ToString(),
					Jwtid = token.Id,
					UserId = user.UserId,
					CreationDate = DateTime.Now,
					ExpiryDate =DateTime.Now
				};
				_context.RefreshTokens.Add(refreshToken);
				_context.SaveChanges();
				result.RefreshToken = refreshToken.RefreshToken1;
				result.Success = true;
				return result;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
