using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using SecurityService.Security;
using System.IdentityModel.Tokens.Jwt;
using SecurityService.Model;

namespace SecurityService.Controllers
{
    [Produces("application/json")]
    [Route("api/Notes")]
    public class NotesController : Controller
    {
        private readonly DataContext _context;
        private readonly ITokenHandler<JwtSecurityToken> _jwtHandler;

        public NotesController(DataContext context, ITokenHandler<JwtSecurityToken> jwtHandler)
        {
            _context = context;
            _jwtHandler = jwtHandler;
        }

        // GET: api/Notes
        [HttpGet]
        public ObjectResult GetNotes([FromHeader] string token)
        {
            int user = GetUser(token);
            if(user == -1)
            {
                return new NotFoundObjectResult("Unauthorized!");
            }
            return new OkObjectResult(_context.Notes.Where(note => note.UserId == user));
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromHeader] string token, [FromRoute] int id)
        {
            int user = GetUser(token);
            if (user == -1)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Notes.SingleOrDefaultAsync(m => m.Id == id && m.UserId == user);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // PUT: api/Notes/update
        [HttpPut("update")]
        public async Task<IActionResult> PutNote([FromHeader] string token, [FromBody] NoteViewModel note)
        {
            int user = GetUser(token);
            if (user == -1)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbNote = await _context.Notes.SingleOrDefaultAsync(m => m.Id == note.Id & m.UserId == user);
            dbNote.Modified = DateTime.Now;
            dbNote.Title = note.Title;
            dbNote.Content = note.Content;
          
            _context.Entry(dbNote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(dbNote.Id, user))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Notes/add
        [HttpPost("add")]
        public async Task<IActionResult> PostNote([FromHeader] string token, [FromBody] NoteViewModel note)
        {
            int user = GetUser(token);
            if (user == -1)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbNote = new Note {
                 Title = note.Title,
                 Content = note.Content,
                 Created = DateTime.Now,
                 UserId = user
            };


            _context.Notes.Add(dbNote);
            await _context.SaveChangesAsync();

            return Ok(dbNote);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromHeader] string token, [FromRoute] int id)
        {
            int user = GetUser(token);
            if (user == -1)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Notes.SingleOrDefaultAsync(m => m.Id == id && m.UserId == user);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        private bool NoteExists(int id, int user)
        {
            return _context.Notes.Any(e => e.Id == id && e.UserId == user);
        }

        private int GetUser(string token)
        {
            var verifiedToken = _jwtHandler.VerifyToken(token);
            if (verifiedToken == null)
            {
                return -1;
            }
            return Int32.Parse(verifiedToken.Payload["user"].ToString());
        }
    }
}