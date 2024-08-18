using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Enrollments
    {
        public int EnrollmentID { get; set; }

        public int StudentID { get; set; }

        public int SubjectID { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public bool State { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
