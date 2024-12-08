using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class CourseSelectionHistory
    {
        [Key]
        public int StudentID { get; set; }  // Öğrenci ID'si
        public DateTime SelectionDate { get; set; }  // Seçim tarihi

        // İlişkiler
        public virtual Student? Student { get; set; }
    }
}
