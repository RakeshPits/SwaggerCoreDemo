using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;

namespace AuthenticationInCoreDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]        
        [Authorize(Roles = "Role2")]       
        public IEnumerable<WeatherForecast> Get()
        {
            var identities = HttpContext.User.Identities;
            var gmailClaims = identities.FirstOrDefault(s => s.AuthenticationType == "Gmail-Claims");
            var userName = gmailClaims.Claims.Where(c => c.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("Authenticat")]      
        public IActionResult Authenticat()
        {
            var gmailClaims = GetGmailClaims();
            var gmailIdentity = new ClaimsIdentity(gmailClaims, "Gmail-Claims");
            var faceBookClaims = GetFaceBookClaims();            
            var faceBookIdentity = new ClaimsIdentity(faceBookClaims, "FaceBook-Claims");

            var userPrincipal = new ClaimsPrincipal(new[] { gmailIdentity, faceBookIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return Ok("Authentication Cookie granded");
        }

        private IReadOnlyList<Claim> GetGmailClaims()
        {
           return new List<Claim>
            {
                new Claim(ClaimTypes.Name, "User1"),
                new Claim(ClaimTypes.Email, "User1@gmail.com"),
                new Claim(ClaimTypes.Role, "Role1"),
                new Claim("Permission_Grant", "Yes"),
            };
        }

        private IReadOnlyList<Claim> GetFaceBookClaims()
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, "User2"),
                new Claim(ClaimTypes.Email, "User1@facebook.com"),
                new Claim(ClaimTypes.Role, "Role2"),
                new Claim("Permission_Grant", "Yes"),
            };
        }
    }
}
