using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranscriptsController : ControllerBase
    {
        private readonly RepositoryDbContext _context;

        public TranscriptsController(RepositoryDbContext context)
        {
            _context = context;
        }

        // GET: api/Transcripts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transcripts>>> GetTranscripts()
        {
            return await _context.Transcripts.ToListAsync();
        }

        // GET: api/Transcripts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transcripts>> GetTranscripts(int id)
        {
            var transcripts = await _context.Transcripts.FindAsync(id);

            if (transcripts == null)
            {
                return NotFound();
            }

            return transcripts;
        }

        // PUT: api/Transcripts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTranscripts(int id, Transcripts transcripts)
        {
            if (id != transcripts.TranscriptID)
            {
                return BadRequest();
            }

            _context.Entry(transcripts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TranscriptsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transcripts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transcripts>> PostTranscripts(Transcripts transcripts)
        {
            _context.Transcripts.Add(transcripts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTranscripts", new { id = transcripts.TranscriptID }, transcripts);
        }

        // DELETE: api/Transcripts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTranscripts(int id)
        {
            var transcripts = await _context.Transcripts.FindAsync(id);
            if (transcripts == null)
            {
                return NotFound();
            }

            _context.Transcripts.Remove(transcripts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TranscriptsExists(int id)
        {
            return _context.Transcripts.Any(e => e.TranscriptID == id);
        }
    }
}
