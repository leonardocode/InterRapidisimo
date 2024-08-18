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
    public class ProfessorBL
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProfessorBL> _logger;

        public ProfessorBL(IUnitOfWork unitOfWork, ILogger<ProfessorBL> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<(List<Professors> Professors, string ErrorMessage)> GetAll()
        {
            try
            {
                var professors = await _unitOfWork.Professors.GetAllAsync();
                return (new List<Professors>(professors), string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Professors, método GetAll");
                return (null, $"Error al listar los profesores: {ex.Message}");
            }
        }

        public async Task<(Professors Professor, string ErrorMessage)> GetById(int id)
        {
            try
            {
                var professor = await _unitOfWork.Professors.GetByIdAsync(id);
                return professor != null
                    ? (professor, string.Empty)
                    : (null, $"No se encontró el profesor con ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Professors, método GetById");
                return (null, $"Error al obtener el profesor: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Insert(Professors professor)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(professor.FirstName))
                    return (false, "El campo nombre es obligatorio");

                if (string.IsNullOrWhiteSpace(professor.LastName))
                    return (false, "El campo apellido es obligatorio");

                if (string.IsNullOrWhiteSpace(professor.Email))
                    return (false, "El campo email es obligatorio");

                await _unitOfWork.Professors.AddAsync(professor);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Professors, método Insert");
                return (false, $"Error al insertar el profesor: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Update(Professors professor)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(professor.FirstName))
                    return (false, "El campo nombre es obligatorio");

                if (string.IsNullOrWhiteSpace(professor.LastName))
                    return (false, "El campo apellido es obligatorio");

                if (string.IsNullOrWhiteSpace(professor.Email))
                    return (false, "El campo email es obligatorio");

                await _unitOfWork.Professors.UpdateAsync(professor);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Professors, método Update");
                return (false, $"Error al actualizar el profesor: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Delete(int id)
        {
            try
            {
                var professor = await _unitOfWork.Professors.GetByIdAsync(id);
                if (professor == null)
                    return (false, $"No se encontró el profesor con ID {id}");

                await _unitOfWork.Professors.RemoveAsync(professor);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Professors, método Delete");
                return (false, $"Error al eliminar el profesor: {ex.Message}");
            }
        }
    }
}
