namespace WebApplication8.Models
{
    public class CourseQuotaResponse
    {
        public int CourseId { get; set; }
        public int Quota { get; set; }
        public int RemainingQuota { get; set; }
        public object Course { get; set; } // Bu alanda course verisi null geliyor, gerekirse daha fazla özelleştirebilirsiniz
    }

}
