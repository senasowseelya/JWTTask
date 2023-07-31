﻿using Newtonsoft.Json;

namespace JWTDemo.Models
{
	public class TokenModel
	{
		[JsonProperty("token")]
		public string Token { get; set; }
		[JsonProperty("refreshToken")]
		public string RefreshToken { get; set; }
	}

	public class AuthenticationResult : TokenModel
	{
		public bool Success { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}
