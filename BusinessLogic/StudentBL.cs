using Data.IUnitOfWork;
using Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class StudentBL
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentBL> _logger;

        public StudentBL(IUnitOfWork unitOfWork, ILogger<StudentBL> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<(List<Students> Students, string ErrorMessage)> GetAll()
        {
            try
            {
                var students = await _unitOfWork.Students.GetAllAsync();
                return (new List<Students>(students), string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Students, método GetAll");
                return (null, $"Error al listar los estudiantes: {ex.Message}");
            }
        }

        public async Task<(Students Student, string ErrorMessage)> GetById(int id)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(id);
                return student != null
                    ? (student, string.Empty)
                    : (null, $"No se encontró el estudiante con ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Students, método GetById");
                return (null, $"Error al obtener el estudiante: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Insert(Students student)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(student.FirstName))
                    return (false, "El campo nombre es obligatorio");

                if (string.IsNullOrWhiteSpace(student.LastName))
                    return (false, "El campo apellido es obligatorio");

                if (string.IsNullOrWhiteSpace(student.Email))
                    return (false, "El campo email es obligatorio");

                await _unitOfWork.Students.AddAsync(student);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Students, método Insert");
                return (false, $"Error al insertar el estudiante: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Update(Students student)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(student.FirstName))
                    return (false, "El campo nombre es obligatorio");

                if (string.IsNullOrWhiteSpace(student.LastName))
                    return (false, "El campo apellido es obligatorio");

                if (string.IsNullOrWhiteSpace(student.Email))
                    return (false, "El campo email es obligatorio");

                await _unitOfWork.Students.UpdateAsync(student);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Students, método Update");
                return (false, $"Error al actualizar el estudiante: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Delete(int id)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(id);
                if (student == null)
                    return (false, $"No se encontró el estudiante con ID {id}");

                await _unitOfWork.Students.RemoveAsync(student);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Students, método Delete");
                return (false, $"Error al eliminar el estudiante: {ex.Message}");
            }
        }
    }
}
