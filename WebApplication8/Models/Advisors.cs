using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace WebApplication8.Models
{
    public class Advisors
    {

        [Key]
        public int AdvisorID { get; set; } //

 public string FullName { get; set; }
        // Akademisyen Adı Soyadı
        public string Title { get; set; } //

 public string Department
        {
            get; set;
        } // Bölüm
        public string Email { get; set; } //

 // Navigation Property
 public ICollection<Student>? Students
        { get; set; }
    }
}
