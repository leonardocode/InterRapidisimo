using BusinessLogic;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WSInterRapidissimo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorsController : ControllerBase
    {
        private readonly ProfessorBL _professorBL;
        private readonly ILogger<ProfessorsController> _logger;

        public ProfessorsController(ProfessorBL professorBL, ILogger<ProfessorsController> logger)
        {
            _professorBL = professorBL;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var (professors, errorMessage) = await _professorBL.GetAll();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            return Ok(professors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (professor, errorMessage) = await _professorBL.GetById(id);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return NotFound(errorMessage);
            }
            return Ok(professor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Professors professor)
        {
            var (success, errorMessage) = await _professorBL.Insert(professor);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            return CreatedAtAction(nameof(GetById), new { id = professor.ProfessorID }, professor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Professors professor)
        {
            if (id != professor.ProfessorID)
            {
                return BadRequest("El ID en la URL no coincide con el ID del profesor en el cuerpo");
            }
            var (success, errorMessage) = await _professorBL.Update(professor);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, errorMessage) = await _professorBL.Delete(id);
            if (!success)
            {
                return NotFound(errorMessage);
            }
            return NoContent();
        }
    }
}
