using DBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL
{
    public class RequestDB : BaseDB<Request>
    {
        protected override string GetTableName() => "requests";  
        protected override string GetPrimaryKeyName() => "request_id";

        protected override async Task<Request> CreateModelAsync(object[] row)
        {
            return new Request
            {
                RequestId = Convert.ToInt32(row[0]),
                TeacherId = Convert.ToInt32(row[1]),
                StudentId = Convert.ToInt32(row[2]),
                Subject = row[3]?.ToString(),
                RequestedDateTime = Convert.ToDateTime(row[4]),
                DurationMinutes = Convert.ToInt32(row[5]),
                Notes = row[6]?.ToString(),
                Status = row[7]?.ToString(),
                CreatedAt = Convert.ToDateTime(row[8]),
                RespondedAt = row[9] != DBNull.Value ? Convert.ToDateTime(row[9]) : null
            };
        }

        public async Task<Request> CreateRequestAsync(Request request)
        {
            Dictionary<string, object> values = new()
            {
                { "teacher_id", request.TeacherId },
                { "student_id", request.StudentId },
                { "subject", request.Subject },
                { "requested_datetime", request.RequestedDateTime },
                { "duration_minutes", request.DurationMinutes },
                { "notes", request.Notes ?? "" },
                { "status", "Pending" },
                { "created_at", DateTime.Now }
            };

            return await base.InsertGetObjAsync(values);
        }

        public async Task<List<Request>> GetTeacherRequestsAsync(int teacherId)
        {
            Dictionary<string, object> filter = new() { { "teacher_id", teacherId } };
            var requests = await SelectAllAsync(filter);
            return requests.OrderByDescending(r => r.CreatedAt).ToList();
        }

        public async Task<List<Request>> GetStudentRequestsAsync(int studentId)
        {
            Dictionary<string, object> filter = new() { { "student_id", studentId } };
            var requests = await SelectAllAsync(filter);
            return requests.OrderByDescending(r => r.CreatedAt).ToList();
        }
        public async Task<Request?> GetByIdAsync(int requestId)
        {
            Dictionary<string, object> filter = new()
            {
                { "request_id", requestId }
            };

            var requests = await SelectAllAsync(filter);
            return requests.FirstOrDefault();
        }
        public async Task<int> UpdateRequestStatusAsync(int requestId, string status)
        {
            Dictionary<string, object> values = new()
            {
                { "status", status },
                { "responded_at", DateTime.Now }
            };

            Dictionary<string, object> filter = new()
            {
                { "request_id", requestId }
            };

            return await base.UpdateAsync(values, filter);
        }

        public async Task<int> GetPendingCountAsync(int teacherId)
        {
            Dictionary<string, object> filter = new()
            {
                { "teacher_id", teacherId },
                { "status", "Pending" }
            };
            var requests = await SelectAllAsync(filter);
            return requests.Count;
        }

        public async Task<List<Request>> GetPendingRequestsAsync(int teacherId)
        {
            Dictionary<string, object> filter = new()
            {
                { "teacher_id", teacherId },
                { "status", "Pending" }
            };
            return await SelectAllAsync(filter);
        }
    }
}
