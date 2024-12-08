namespace WebApplication8.DTO
{
    public class NonConfirmedSelectionDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }  // studentID alanı
        public int CourseId { get; set; }   // courseID alanı
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CourseName { get; set; }
    }
}
