using BusinessLogic;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WSInterRapidissimo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly EnrollmentBL _enrollmentBL;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(EnrollmentBL enrollmentBL, ILogger<EnrollmentsController> logger)
        {
            _enrollmentBL = enrollmentBL;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var (enrollments, errorMessage) = await _enrollmentBL.GetAll();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (enrollment, errorMessage) = await _enrollmentBL.GetById(id);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return NotFound(errorMessage);
            }
            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Enrollments enrollment)
        {
            var (success, errorMessage) = await _enrollmentBL.Insert(enrollment);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            return CreatedAtAction(nameof(GetById), new { id = enrollment.EnrollmentID }, enrollment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Enrollments enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return BadRequest("El ID en la URL no coincide con el ID de la inscripción en el cuerpo");
            }
            var (success, errorMessage) = await _enrollmentBL.Update(enrollment);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, errorMessage) = await _enrollmentBL.Delete(id);
            if (!success)
            {
                return NotFound(errorMessage);
            }
            return NoContent();
        }

        [HttpGet("student/{studentId}/cost")]
        public async Task<IActionResult> GetTotalCost(int studentId)
        {
            var (totalCostUSD, totalCostEUR, errorMessage) = await _enrollmentBL.CalculateTotalCost(studentId);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            return Ok(new { TotalCostUSD = totalCostUSD, TotalCostEUR = totalCostEUR });
        }

        [HttpGet("student/{studentId}/subject/{subjectId}/classmates")]
        public async Task<IActionResult> GetClassmates(int studentId, int subjectId)
        {
            var (classmates, errorMessage) = await _enrollmentBL.GetClassmates(studentId, subjectId);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            return Ok(classmates);
        }
    }
}
