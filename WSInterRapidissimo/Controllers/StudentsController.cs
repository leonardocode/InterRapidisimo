using BusinessLogic;
using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WSInterRapidissimo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentBL _studentsBL;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(StudentBL studentsBL, ILogger<StudentsController> logger)
        {
            _studentsBL = studentsBL;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var (students, errorMessage) = await _studentsBL.GetAll();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (student, errorMessage) = await _studentsBL.GetById(id);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return NotFound(errorMessage);
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Students student)
        {
            var (success, errorMessage) = await _studentsBL.Insert(student);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            return CreatedAtAction(nameof(GetById), new { id = student.StudentID }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Students student)
        {
            if (id != student.StudentID)
            {
                return BadRequest("El ID en la URL no coincide con el ID del estudiante en el cuerpo");
            }

            var (success, errorMessage) = await _studentsBL.Update(student);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, errorMessage) = await _studentsBL.Delete(id);
            if (!success)
            {
                return NotFound(errorMessage);
            }
            return NoContent();
        }
    }
}
