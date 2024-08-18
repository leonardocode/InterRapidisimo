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
    public class StudentRepository : Repository<Students>, IStudentRepository
    {
        public StudentRepository(Conexion conexion, ILogger<StudentRepository> logger)
             : base(conexion, logger)
        {
        }

        public override async Task<Students> GetByIdAsync(int id)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 2);
                _conexion.adicionarParametro("id", id);
                DataSet ds = await _conexion.ejecutarProcedimiento("sp_students");

                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    return new Students
                    {
                        StudentID = Convert.ToInt32(row["StudentID"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        Email = row["Email"].ToString(),
                        State = Convert.ToBoolean(row["State"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estudiante por ID");
                throw;
            }
        }

        public override async Task<IEnumerable<Students>> GetAllAsync()
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 1);
                DataSet ds = await _conexion.ejecutarProcedimiento("sp_students");

                List<Students> students = new List<Students>();
                if (ds?.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        students.Add(new Students
                        {
                            StudentID = Convert.ToInt32(row["StudentID"]),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Email = row["Email"].ToString(),
                            State = Convert.ToBoolean(row["State"])
                        });
                    }
                }
                return students;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los estudiantes");
                throw;
            }
        }

        public override async Task AddAsync(Students entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 3);
                _conexion.adicionarParametro("firstName", entity.FirstName);
                _conexion.adicionarParametro("lastName", entity.LastName);
                _conexion.adicionarParametro("email", entity.Email);
                await _conexion.ejecutarProcedimiento("sp_students");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar estudiante");
                throw;
            }
        }

        public override async Task UpdateAsync(Students entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 4);
                _conexion.adicionarParametro("id", entity.StudentID);
                _conexion.adicionarParametro("firstName", entity.FirstName);
                _conexion.adicionarParametro("lastName", entity.LastName);
                _conexion.adicionarParametro("email", entity.Email);
                _conexion.adicionarParametro("state", entity.State);
                await _conexion.ejecutarProcedimiento("sp_students");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estudiante");
                throw;
            }
        }

        public override async Task RemoveAsync(Students entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 5);
                _conexion.adicionarParametro("id", entity.StudentID);
                await _conexion.ejecutarProcedimiento("sp_students");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar estudiante");
                throw;
            }
        }

        public async Task<Students> GetByEmailAsync(string email)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 6);
                _conexion.adicionarParametro("email", email);
                DataSet ds = await _conexion.ejecutarProcedimiento("sp_students");

                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    return new Students
                    {
                        StudentID = Convert.ToInt32(row["StudentID"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        Email = row["Email"].ToString(),
                        State = Convert.ToBoolean(row["State"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estudiante por email");
                throw;
            }
        }

    }
}
