using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication8.Models
{
    public class StudentCourseSelections
    {
        [Key]
        public int SelectionID { get; set; }

        public int CourseID { get; set; }

        [Column(TypeName = "date")]
        public DateTime SelectionDate { get; set; }

        public bool IsApproved { get; set; }
        public int StudentID { get; set; }

        // Navigation properties
        [ForeignKey("StudentID")]
        public virtual Student? Student { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course? Course { get; set; }
    }
}