using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
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
    public class NonConfirmedSelectionsController : ControllerBase
    {
        private readonly RepositoryDbContext _context;

        public NonConfirmedSelectionsController(RepositoryDbContext context)
        {
            _context = context;
        }

        // GET: api/NonConfirmedSelections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NonConfirmedSelections>>> GetNonConfirmedSelections()
        {
            var nonConfirmedSelections = await _context.NonConfirmedSelections
                .Include(ns => ns.Student)
                .Include(ns => ns.Course)
                .Select(ns => new
                {
                    ns.Id,
                    ns.Student.StudentID,    // StudentId'yi ekledik
                    ns.Student.FirstName,
                    ns.Student.LastName,
                    ns.Course.CourseName,
                    ns.Course.CourseID       // CourseId'yi de ekledik
                })
                .ToListAsync();

            return Ok(nonConfirmedSelections);
        }



        // GET: api/NonConfirmedSelections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NonConfirmedSelections>> GetNonConfirmedSelections(int id)
        {
            var nonConfirmedSelections = await _context.NonConfirmedSelections.FindAsync(id);

            if (nonConfirmedSelections == null)
            {
                return NotFound();
            }

            return nonConfirmedSelections;
        }

        // PUT: api/NonConfirmedSelections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNonConfirmedSelections(int id, NonConfirmedSelections nonConfirmedSelections)
        {
            if (id != nonConfirmedSelections.Id)
            {
                return BadRequest();
            }

            _context.Entry(nonConfirmedSelections).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NonConfirmedSelectionsExists(id))
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

        // POST: api/NonConfirmedSelections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NonConfirmedSelections>> PostNonConfirmedSelections(NonConfirmedSelections nonConfirmedSelections)
        {
            _context.NonConfirmedSelections.Add(nonConfirmedSelections);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNonConfirmedSelections", new { id = nonConfirmedSelections.Id }, nonConfirmedSelections);
        }

        // DELETE: api/NonConfirmedSelections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNonConfirmedSelections(int id)
        {
            var nonConfirmedSelections = await _context.NonConfirmedSelections.FindAsync(id);
            if (nonConfirmedSelections == null)
            {
                return NotFound();
            }

            _context.NonConfirmedSelections.Remove(nonConfirmedSelections);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NonConfirmedSelectionsExists(int id)
        {
            return _context.NonConfirmedSelections.Any(e => e.Id == id);
        }
    }
}
