using BusinessLogic;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WSInterRapidissimo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly SubjectBL _subjectBL;
        private readonly ILogger<SubjectsController> _logger;

        public SubjectsController(SubjectBL subjectBL, ILogger<SubjectsController> logger)
        {
            _subjectBL = subjectBL;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var (subjects, errorMessage) = await _subjectBL.GetAll();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (subject, errorMessage) = await _subjectBL.GetById(id);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return NotFound(errorMessage);
            }
            return Ok(subject);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Subjects subject)
        {
            var (success, errorMessage) = await _subjectBL.Insert(subject);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            return CreatedAtAction(nameof(GetById), new { id = subject.SubjectID }, subject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Subjects subject)
        {
            if (id != subject.SubjectID)
            {
                return BadRequest("El ID en la URL no coincide con el ID de la materia en el cuerpo");
            }
            var (success, errorMessage) = await _subjectBL.Update(subject);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, errorMessage) = await _subjectBL.Delete(id);
            if (!success)
            {
                return NotFound(errorMessage);
            }
            return NoContent();
        }
    }
}
