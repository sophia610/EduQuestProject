using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public Request() { }
        public Request(int requestId, int teacherId, int studentId, string subject, DateTime requestedDateTime, int durationMinutes, string notes, string status, DateTime createdAt, DateTime? respondedAt)
        {
            RequestId = requestId;
            TeacherId = teacherId;
            StudentId = studentId;
            Subject = subject;
            RequestedDateTime = requestedDateTime;
            DurationMinutes = durationMinutes;
            Notes = notes;
            Status = status;
            CreatedAt = createdAt;
            RespondedAt = respondedAt;
        }
    }
}
