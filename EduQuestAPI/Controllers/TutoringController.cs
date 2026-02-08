using Microsoft.AspNetCore.Mvc;
using DBL;
using Models;

namespace EduQuestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TutoringController : ControllerBase
    {
        private readonly RequestDB _db;
        private readonly CustomerDB _customerDb;

        public TutoringController()
        {
            _db = new RequestDB();
            _customerDb = new CustomerDB();
        }

        [HttpPost("request")]
        public async Task<ActionResult<Request>> CreateRequest([FromBody] Request request)
        {
            try
            {
                var result = await _db.CreateRequestAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult> GetTeacherRequests(int teacherId)
        {
            try
            {
                var requests = await _db.GetTeacherRequestsAsync(teacherId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{requestId}/status")]
        public async Task<ActionResult> UpdateStatus(int requestId, [FromBody] StatusUpdate update)
        {
            try
            {
                var result = await _db.UpdateRequestStatusAsync(requestId, update.Status);
                return Ok(new { success = result > 0 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class StatusUpdate
    {
        public string Status { get; set; }
    }
}