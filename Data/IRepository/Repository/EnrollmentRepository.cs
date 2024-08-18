using Data.IRepository;
using Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IRepository.Repository
{
    public class EnrollmentRepository : Repository<Enrollments>, IEnrollmentRepository
    {
        public EnrollmentRepository(Conexion conexion, ILogger<Repository<Enrollments>> logger) : base(conexion, logger)
        {
        }
        public override async Task<Enrollments> GetByIdAsync(int id)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 2);
                _conexion.adicionarParametro("id", id);
                DataSet ds = await _conexion.ejecutarProcedimiento("sp_Enrollments");

                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    return new Enrollments
                    {
                        EnrollmentID = Convert.ToInt32(row["EnrollmentID"]),
                        StudentID = Convert.ToInt32(row["StudentID"]),
                        SubjectID = Convert.ToInt32(row["SubjectID"]),
                        State = Convert.ToBoolean(row["State"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener inscripción por ID");
                throw;
            }
        }

        public override async Task<IEnumerable<Enrollments>> GetAllAsync()
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 1);
                DataSet ds = await _conexion.ejecutarProcedimiento("sp_Enrollments");

                List<Enrollments> enrollments = new List<Enrollments>();
                if (ds?.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        enrollments.Add(new Enrollments
                        {
                            EnrollmentID = Convert.ToInt32(row["EnrollmentID"]),
                            StudentID = Convert.ToInt32(row["StudentID"]),
                            SubjectID = Convert.ToInt32(row["SubjectID"]),
                            State = Convert.ToBoolean(row["State"])
                        });
                    }
                }
                return enrollments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las inscripciones");
                throw;
            }
        }

        public override async Task AddAsync(Enrollments entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 3);
                _conexion.adicionarParametro("studentId", entity.StudentID);
                _conexion.adicionarParametro("subjectId", entity.SubjectID);
                await _conexion.ejecutarProcedimiento("sp_Enrollments");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar inscripción");
                throw;
            }
        }

        public override async Task UpdateAsync(Enrollments entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 4);
                _conexion.adicionarParametro("id", entity.EnrollmentID);
                _conexion.adicionarParametro("studentId", entity.StudentID);
                _conexion.adicionarParametro("subjectId", entity.SubjectID);
                _conexion.adicionarParametro("state", entity.State);
                await _conexion.ejecutarProcedimiento("sp_Enrollments");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar inscripción");
                throw;
            }
        }

        public override async Task RemoveAsync(Enrollments entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 5);
                _conexion.adicionarParametro("id", entity.EnrollmentID);
                await _conexion.ejecutarProcedimiento("sp_Enrollments");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar inscripción");
                throw;
            }
        }
    }
}
