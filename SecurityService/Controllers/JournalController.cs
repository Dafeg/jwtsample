using Data;
using Microsoft.AspNetCore.Mvc;
using SecurityService.Helpers;
using SecurityService.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityService.Controllers
{
    [Produces("application/json")]
    [Route("api/Journal")]
    public class JournalController : Controller
    {
        private readonly DataContext _context;
        private readonly ITokenHandler<JwtSecurityToken> _jwtHandler;

        public JournalController(DataContext context, ITokenHandler<JwtSecurityToken> jwtHandler)
        {
            _context = context;
            _jwtHandler = jwtHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetJournalEntries([FromHeader] string token)
        {
            var verifiedToken = _jwtHandler.VerifyToken(token);
            if(verifiedToken == null)
            {
                _context.Journals.Add(JournalEntryBuilder.CreateEntry("Get journal entries action", false, "Unauthorized"));
                await _context.SaveChangesAsync();
                return Unauthorized();
            }

            if(verifiedToken.Payload["role"].ToString() != "Admin")
            {
                _context.Journals.Add(JournalEntryBuilder.CreateEntry("Get journal entries action", false, 
                    verifiedToken.Payload["user"].ToString()));
                await _context.SaveChangesAsync();
                return BadRequest("Access denied");
            }

            _context.Journals.Add(JournalEntryBuilder.CreateEntry("Get journal entries action", true,
                    verifiedToken.Payload["user"].ToString()));
            await _context.SaveChangesAsync();

            return Ok(_context.Journals);
        }
    }
}
