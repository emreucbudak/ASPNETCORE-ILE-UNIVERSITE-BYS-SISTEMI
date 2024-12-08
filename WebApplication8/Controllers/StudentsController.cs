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
    public class StudentsController : ControllerBase
    {
        private readonly RepositoryDbContext _context;

        public StudentsController(RepositoryDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            var studentsWithAdvisorAndCourses = await _context.Students
                .Select(s => new
                {
                    s.StudentID,
                    s.FirstName,
                    s.LastName,
                    s.Email,
                    Advisor = new
                    {
                        s.Advisors.FullName, // Danışmanın adı
                        s.Advisors.Title, // Danışmanın unvanı
                        s.Advisors.Department // Danışmanın bölümü
                    },
                    Courses = s.StudentCourseSelections.Select(sc => new
                    {
                        sc.CourseID,
                        sc.Course.CourseName, // Kurs adı
                        sc.SelectionDate
                    }).ToList()
                })
                .ToListAsync();

            return Ok(studentsWithAdvisorAndCourses);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var studentWithAdvisorAndCourses = await _context.Students
                .Where(s => s.StudentID == id)
                .Select(s => new
                {
                    s.StudentID,
                    s.FirstName,
                    s.LastName,
                    s.Email,
                    Advisor = new
                    {
                        s.Advisors.FullName, // Danışmanın adı
                        s.Advisors.Title, // Danışmanın unvanı
                        s.Advisors.Department // Danışmanın bölümü
                    },
                    Courses = s.StudentCourseSelections.Select(sc => new
                    {
                        sc.CourseID,
                        sc.Course.CourseName, // Kurs adı
                        sc.SelectionDate
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (studentWithAdvisorAndCourses == null)
            {
                return NotFound(); // Öğrenci bulunamazsa 404 döndür
            }

            return Ok(studentWithAdvisorAndCourses); // Öğrenci ve ilişkili veriler başarılı şekilde döndürülür
        }


        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentID }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourseSelections) // İlişkili kayıtları getir
                .FirstOrDefaultAsync(s => s.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            // Önce ilişkili kayıtları sil
            _context.StudentCourseSelections.RemoveRange(student.StudentCourseSelections);

            // Ardından öğrenciyi sil
            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}
