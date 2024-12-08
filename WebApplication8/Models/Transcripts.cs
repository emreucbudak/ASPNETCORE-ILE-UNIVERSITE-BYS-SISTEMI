using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class Transcripts
    {
        [Key]
        public int TranscriptID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public string? Grade { get; set; }
        public string? Semester { get; set; }
    }

}
