using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class SystemLog
    {
        [Key]
        public Guid Id { get; set; }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
        public DateTime LogTime { get; set; }

    }
}
