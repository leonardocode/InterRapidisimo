using Data.IUnitOfWork;
using Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class EnrollmentBL
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EnrollmentBL> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public EnrollmentBL(IUnitOfWork unitOfWork, ILogger<EnrollmentBL> logger, IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(List<Enrollments> Enrollments, string ErrorMessage)> GetAll()
        {
            try
            {
                var enrollments = await _unitOfWork.Enrollments.GetAllAsync();
                return (new List<Enrollments>(enrollments), string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Enrollments, método GetAll");
                return (null, $"Error al listar las inscripciones: {ex.Message}");
            }
        }

        public async Task<(Enrollments Enrollment, string ErrorMessage)> GetById(int id)
        {
            try
            {
                var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
                return enrollment != null
                    ? (enrollment, string.Empty)
                    : (null, $"No se encontró la inscripción con ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Enrollments, método GetById");
                return (null, $"Error al obtener la inscripción: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Insert(Enrollments enrollment)
        {
            try
            {
                var studentEnrollments = await _unitOfWork.Enrollments.FindAsync(e => e.StudentID == enrollment.StudentID && e.State);
                if (studentEnrollments.Count() >= 3)
                {
                    return (false, "El estudiante ya está inscrito en el máximo de materias permitidas (3)");
                }

                var subject = await _unitOfWork.Subjects.GetByIdAsync(enrollment.SubjectID);
                if (subject == null)
                {
                    return (false, "La materia no existe");
                }

                var professorSubjects = await _unitOfWork.Subjects.FindAsync(s => s.ProfessorID == subject.ProfessorID);
                var professorEnrollments = await _unitOfWork.Enrollments.FindAsync(e =>
                    e.StudentID == enrollment.StudentID &&
                    professorSubjects.Select(ps => ps.SubjectID).Contains(e.SubjectID) &&
                    e.State);

                if (professorEnrollments.Any())
                {
                    return (false, "El estudiante ya tiene una clase con este profesor");
                }

                await _unitOfWork.Enrollments.AddAsync(enrollment);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Enrollments, método Insert");
                return (false, $"Error al insertar la inscripción: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Update(Enrollments enrollment)
        {
            try
            {
                await _unitOfWork.Enrollments.UpdateAsync(enrollment);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Enrollments, método Update");
                return (false, $"Error al actualizar la inscripción: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Delete(int id)
        {
            try
            {
                var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
                if (enrollment == null)
                    return (false, $"No se encontró la inscripción con ID {id}");

                await _unitOfWork.Enrollments.RemoveAsync(enrollment);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Enrollments, método Delete");
                return (false, $"Error al eliminar la inscripción: {ex.Message}");
            }
        }

        public async Task<(decimal TotalCostUSD, decimal TotalCostEUR, string ErrorMessage)> CalculateTotalCost(int studentId)
        {
            try
            {
                var enrollments = await _unitOfWork.Enrollments.FindAsync(e => e.StudentID == studentId && e.State);
                var subjectIds = enrollments.Select(e => e.SubjectID).ToList();
                var subjects = await _unitOfWork.Subjects.FindAsync(s => subjectIds.Contains(s.SubjectID));

                decimal totalCostUSD = subjects.Sum(s => s.Credits * 150m);

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://api.frankfurter.app/latest?from=USD&to=EUR&amount={totalCostUSD}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(content);

                decimal totalCostEUR = result["rates"].GetProperty("EUR").GetDecimal();

                return (totalCostUSD, totalCostEUR, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular el costo total");
                return (0, 0, $"Error al calcular el costo total: {ex.Message}");
            }
        }

        public async Task<(List<string> Classmates, string ErrorMessage)> GetClassmates(int studentId, int subjectId)
        {
            try
            {
                var classmates = await _unitOfWork.Enrollments.FindAsync(e =>
                    e.SubjectID == subjectId &&
                    e.StudentID != studentId &&
                    e.State);

                var studentIds = classmates.Select(c => c.StudentID).ToList();
                var students = await _unitOfWork.Students.FindAsync(s => studentIds.Contains(s.StudentID));

                var classmateNames = students.Select(s => $"{s.FirstName} {s.LastName}").ToList();

                return (classmateNames, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los compañeros de clase");
                return (null, $"Error al obtener los compañeros de clase: {ex.Message}");
            }
        }
    }
}
