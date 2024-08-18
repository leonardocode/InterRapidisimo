using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.IRepository;

namespace Data.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        IProfessorRepository Professors { get; }
        ISubjectRepository Subjects { get; }
        IEnrollmentRepository Enrollments { get; }
        Task<int> CompleteAsync();
    }
}
