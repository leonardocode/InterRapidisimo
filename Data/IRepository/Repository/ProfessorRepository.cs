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
    public class ProfessorRepository : Repository<Professors>, IProfessorRepository
    {
        public ProfessorRepository(Conexion conexion, ILogger<Repository<Professors>> logger) : base(conexion, logger)
        {
        }

        public override async Task<Professors> GetByIdAsync(int id)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 2);
                _conexion.adicionarParametro("id", id);
                DataSet ds = await _conexion.ejecutarProcedimiento("sp_Professors");

                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    return new Professors
                    {
                        ProfessorID = Convert.ToInt32(row["ProfessorID"]),
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
                _logger.LogError(ex, "Error al obtener profesor por ID");
                throw;
            }
        }

        public override async Task<IEnumerable<Professors>> GetAllAsync()
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 1);
                DataSet ds = await _conexion.ejecutarProcedimiento("sp_Professors");

                List<Professors> professors = new List<Professors>();
                if (ds?.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        professors.Add(new Professors
                        {
                            ProfessorID = Convert.ToInt32(row["ProfessorID"]),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Email = row["Email"].ToString(),
                            State = Convert.ToBoolean(row["State"])
                        });
                    }
                }
                return professors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los profesores");
                throw;
            }
        }

        public override async Task AddAsync(Professors entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 3);
                _conexion.adicionarParametro("firstName", entity.FirstName);
                _conexion.adicionarParametro("lastName", entity.LastName);
                _conexion.adicionarParametro("email", entity.Email);
                await _conexion.ejecutarProcedimiento("sp_Professors");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar profesor");
                throw;
            }
        }

        public override async Task UpdateAsync(Professors entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 4);
                _conexion.adicionarParametro("id", entity.ProfessorID);
                _conexion.adicionarParametro("firstName", entity.FirstName);
                _conexion.adicionarParametro("lastName", entity.LastName);
                _conexion.adicionarParametro("email", entity.Email);
                _conexion.adicionarParametro("state", entity.State);
                await _conexion.ejecutarProcedimiento("sp_Professors");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar profesor");
                throw;
            }
        }

        public override async Task RemoveAsync(Professors entity)
        {
            try
            {
                _conexion.ClearParameters();
                _conexion.adicionarParametro("Opcion", 5);
                _conexion.adicionarParametro("id", entity.ProfessorID);
                await _conexion.ejecutarProcedimiento("sp_Professors");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar profesor");
                throw;
            }
        }
    }
}
