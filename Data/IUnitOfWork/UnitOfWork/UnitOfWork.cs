using Data.IRepository;
using Data.IRepository.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IUnitOfWork.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Conexion _conexion;
        private readonly ILoggerFactory _loggerFactory;

        public IStudentRepository Students { get; private set; }
        public IProfessorRepository Professors { get; private set; }
        public ISubjectRepository Subjects { get; private set; }
        public IEnrollmentRepository Enrollments { get; private set; }

        public UnitOfWork(Conexion conexion, ILoggerFactory loggerFactory)
        {
            _conexion = conexion;
            _loggerFactory = loggerFactory;
            Students = new StudentRepository(_conexion, _loggerFactory.CreateLogger<StudentRepository>());
            Professors = new ProfessorRepository(_conexion, _loggerFactory.CreateLogger<ProfessorRepository>());
            Subjects = new SubjectRepository(_conexion, _loggerFactory.CreateLogger<SubjectRepository>());
            Enrollments = new EnrollmentRepository(_conexion, _loggerFactory.CreateLogger<EnrollmentRepository>());
        }

        public async Task<int> CompleteAsync()
        {
            return 0;
        }

        public void Dispose()
        {
        }
    }
}
