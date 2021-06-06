using ElevenFeet;
using ElevenFeet.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ImportantEventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOptions<AppSettings> _appSettings;

        public AccountController(UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings;
        }

        // api/Accounts/Login
        // https://dotnetdetail.net/asp-net-core-5-web-api-token-based-authentication-example-using-jwt/
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(dto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(_appSettings.Value.JwtSecret));

                var token = new JwtSecurityToken(
                    issuer: _appSettings.Value.JwtIssuer,
                    audience: _appSettings.Value.JwtIssuer,
                    expires: DateTime.Now.AddDays(7),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    token = stringToken,
                    expiration = token.ValidTo
                });
            }

            return BadRequest("Something Wrong");
        }

        [HttpGet("IsAuthorize")]
        [Authorize]
        public IActionResult IsAuthorize()
        {
            return Ok();
        }
    }
}
