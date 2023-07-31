using JWTDemo.Models;
using JWTDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTDemo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JWTAuthController : ControllerBase
	{

		public IdentityService _identityService { get; set; }
		public JWTAuthController(IdentityService identityService)
		{
			_identityService = identityService;
		}

		[HttpPost]
		public IActionResult Login(LoginModel login)
		{
			return Ok(_identityService.Login(login));
		}
	}
}
