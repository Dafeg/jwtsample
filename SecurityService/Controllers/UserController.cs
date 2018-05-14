using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Data;
using Microsoft.AspNetCore.Mvc;
using SecurityService.Helpers;
using SecurityService.Model;
using SecurityService.Security;

namespace SecurityService.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly DataContext _context;
        private readonly ITokenHandler<JwtSecurityToken> _jwtHandler;

        public UserController(DataContext context, ITokenHandler<JwtSecurityToken> jwtHandler)
        {
            _context = context;
            _jwtHandler = jwtHandler;
        }
        
        [HttpPost("login")]
        public ObjectResult Authorize([FromBody] UserAuthModel userModel)
        {
            if (!ModelState.IsValid)
            {
                _context.Journals.Add(JournalEntryBuilder.CreateEntry("Authorize", false, "Unauthorized"));
                _context.SaveChanges();

                return new BadRequestObjectResult(ModelState);
            }

            var user = _context.Users.FirstOrDefault(c => c.Email == userModel.Email && c.Password == userModel.Password);

            if(user == null)
            {
                _context.Journals.Add(JournalEntryBuilder.CreateEntry("Authorize", false, "Unauthorized"));
                _context.SaveChanges();

                return new BadRequestObjectResult("Invalid credentials");
            }

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("user", user.Id.ToString()));
            claims.Add(new Claim("role", user.Status.ToString()));
            var accessToken = _jwtHandler.GenerateToken(claims);
            var tokenString = $"{accessToken.EncodedHeader}.{accessToken.EncodedPayload}.{accessToken.RawSignature}";

            user.Modified = DateTime.Now;
            _context.Update(user);
            _context.Journals.Add(JournalEntryBuilder.CreateEntry("Authorize", true, user.Id.ToString()));

            _context.SaveChanges();

            return new OkObjectResult(tokenString);
        }
    }
}