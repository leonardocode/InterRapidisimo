using Data.IRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.IRepository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly Conexion _conexion;
        protected readonly ILogger<Repository<T>> _logger;
        public Repository(Conexion conexion, ILogger<Repository<T>> logger)
        {
            _conexion = conexion;
            _logger = logger;
        }


        public virtual async Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                // Obtener todos los elementos
                var allItems = await GetAllAsync();

                // Compilar la expresión lambda
                var compiledPredicate = predicate.Compile();

                // Filtrar los elementos usando el predicado compilado
                return allItems.Where(compiledPredicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en FindAsync para {typeof(T).Name}");
                throw;
            }
        }

        public virtual async Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task RemoveAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
