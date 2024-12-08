using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication8.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }
        public string CourseCode
        {
            get; set;
        }
        public string CourseName
        {
            get; set;
        }
        public bool IsMandatory { get; set; }
        public int Credit { get; set; }
        public string Department
        {
            get; set;
        }
        // Navigation Properties
        public
       ICollection<StudentCourseSelections>
       StudentCourseSelections
        { get; set; } = new
       List<StudentCourseSelections>();
        public virtual ICollection<NonConfirmedSelections> NonConfirmedSelections { get; set; }

    }
}
