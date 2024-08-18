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
    public class SubjectBL
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubjectBL> _logger;

        public SubjectBL(IUnitOfWork unitOfWork, ILogger<SubjectBL> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<(List<Subjects> Subjects, string ErrorMessage)> GetAll()
        {
            try
            {
                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                return (new List<Subjects>(subjects), string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Subjects, método GetAll");
                return (null, $"Error al listar las materias: {ex.Message}");
            }
        }

        public async Task<(Subjects Subject, string ErrorMessage)> GetById(int id)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
                return subject != null
                    ? (subject, string.Empty)
                    : (null, $"No se encontró la materia con ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Subjects, método GetById");
                return (null, $"Error al obtener la materia: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Insert(Subjects subject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject.SubjectName))
                    return (false, "El nombre de la materia es obligatorio");

                if (subject.Credits <= 0)
                    return (false, "Los créditos deben ser un número positivo");

                await _unitOfWork.Subjects.AddAsync(subject);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Subjects, método Insert");
                return (false, $"Error al insertar la materia: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Update(Subjects subject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject.SubjectName))
                    return (false, "El nombre de la materia es obligatorio");

                if (subject.Credits <= 0)
                    return (false, "Los créditos deben ser un número positivo");

                await _unitOfWork.Subjects.UpdateAsync(subject);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Subjects, método Update");
                return (false, $"Error al actualizar la materia: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Delete(int id)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
                if (subject == null)
                    return (false, $"No se encontró la materia con ID {id}");

                await _unitOfWork.Subjects.RemoveAsync(subject);
                await _unitOfWork.CompleteAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la capa negocio: Subjects, método Delete");
                return (false, $"Error al eliminar la materia: {ex.Message}");
            }
        }
    }
}
