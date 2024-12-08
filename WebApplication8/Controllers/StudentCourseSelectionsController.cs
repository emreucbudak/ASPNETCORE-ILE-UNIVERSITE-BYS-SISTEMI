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
    public class StudentCourseSelectionsController : ControllerBase
    {
        private readonly RepositoryDbContext _context;

        public StudentCourseSelectionsController(RepositoryDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentCourseSelections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCourseSelections>>> GetStudentCourseSelections()
        {
            var studentsWithCourses = await _context.Students
                .Include(s => s.StudentCourseSelections)
                    .ThenInclude(sc => sc.Course) // StudentCourseSelections üzerinden Course ilişkisini yükle
                .Select(s => new
                {
                    s.StudentID,
                    s.FirstName,
                    s.LastName,
                    Courses = s.StudentCourseSelections.Select(sc => new
                    {
                        sc.CourseID,
                        sc.SelectionDate,
                        CourseName = sc.Course.CourseName
                    }).ToList()
                })
                .ToListAsync();


            if (studentsWithCourses == null || !studentsWithCourses.Any())
                return NotFound(new { Message = "No students or course selections found." });

            return Ok(studentsWithCourses);
        }

        // GET: api/StudentCourseSelections/5
        [HttpGet("{studentId}")]
        public IActionResult GetStudentCourseSelections(int studentId)
        {
            // Veritabanından seçimleri alıyoruz
            var selections = _context.StudentCourseSelections
               .Where(s => s.StudentID == studentId)
               .Select(s => new
               {
                   s.Student.StudentID,
                   Course = new
                   {
                       s.Course.CourseCode,
                       s.Course.CourseName,
                       s.Course.Department,
                       s.Course.Credit
                   }
               })
               .ToList();


            // Eğer veri yoksa NotFound döndür
            if (selections == null || !selections.Any())
            {
                return NotFound("Bu öğrenci için ders seçimi bulunamadı.");
            }

            // Veri bulundu, JSON formatında geri dön
            return Ok(selections);
        }

        // PUT: api/StudentCourseSelections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentCourseSelections(int id, StudentCourseSelections studentCourseSelections)
        {
            if (id != studentCourseSelections.SelectionID)
            {
                return BadRequest();
            }

            _context.Entry(studentCourseSelections).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseSelectionsExists(id))
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

        // POST: api/StudentCourseSelections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentCourseSelections>> PostStudentCourseSelections(StudentCourseSelections studentCourseSelections)
        {
            _context.StudentCourseSelections.Add(studentCourseSelections);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentCourseSelections", new { id = studentCourseSelections.SelectionID }, studentCourseSelections);
        }

        // DELETE: api/StudentCourseSelections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentCourseSelections(int id)
        {
            var studentCourseSelections = await _context.StudentCourseSelections.FindAsync(id);
            if (studentCourseSelections == null)
            {
                return NotFound();
            }

            _context.StudentCourseSelections.Remove(studentCourseSelections);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentCourseSelectionsExists(int id)
        {
            return _context.StudentCourseSelections.Any(e => e.SelectionID == id);
        }
    }
}
