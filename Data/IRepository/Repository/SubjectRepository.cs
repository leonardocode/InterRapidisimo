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
    public class SubjectRepository : Repository<Subjects>, ISubjectRepository
    {
        public SubjectRepository(Conexion conexion, ILogger<Repository<Subjects>> logger) : base(conexion, logger)
        {
        }

        public override async Task<Subjects> GetByIdAsync(int id)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 2);
                _conexion.adicionarParametro("id", id);
                DataSet ds = await _conexion.ejecutarProcedimiento("sp_Subjects");

                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    return new Subjects
                    {
                        SubjectID = Convert.ToInt32(row["SubjectID"]),
                        SubjectName = row["SubjectName"].ToString(),
                        Credits = Convert.ToInt32(row["Credits"]),
                        ProfessorID = Convert.ToInt32(row["ProfessorID"]),
                        State = Convert.ToBoolean(row["State"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener materia por ID");
                throw;
            }
        }

        public override async Task<IEnumerable<Subjects>> GetAllAsync()
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 1);
                DataSet ds = await _conexion.ejecutarProcedimiento("sp_Subjects");

                List<Subjects> subjects = new List<Subjects>();
                if (ds?.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        subjects.Add(new Subjects
                        {
                            SubjectID = Convert.ToInt32(row["SubjectID"]),
                            SubjectName = row["SubjectName"].ToString(),
                            Credits = Convert.ToInt32(row["Credits"]),
                            ProfessorID = Convert.ToInt32(row["ProfessorID"]),
                            State = Convert.ToBoolean(row["State"])
                        });
                    }
                }
                return subjects;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las materias");
                throw;
            }
        }

        public override async Task AddAsync(Subjects entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 3);
                _conexion.adicionarParametro("subjectName", entity.SubjectName);
                _conexion.adicionarParametro("credits", entity.Credits);
                _conexion.adicionarParametro("professorId", entity.ProfessorID);
                await _conexion.ejecutarProcedimiento("sp_Subjects");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar materia");
                throw;
            }
        }

        public override async Task UpdateAsync(Subjects entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 4);
                _conexion.adicionarParametro("id", entity.SubjectID);
                _conexion.adicionarParametro("subjectName", entity.SubjectName);
                _conexion.adicionarParametro("credits", entity.Credits);
                _conexion.adicionarParametro("professorId", entity.ProfessorID);
                _conexion.adicionarParametro("state", entity.State);
                await _conexion.ejecutarProcedimiento("sp_Subjects");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar materia");
                throw;
            }
        }

        public override async Task RemoveAsync(Subjects entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 5);
                _conexion.adicionarParametro("id", entity.SubjectID);
                await _conexion.ejecutarProcedimiento("sp_Subjects");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar materia");
                throw;
            }
        }

    }
}
