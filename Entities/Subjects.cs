using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Subjects
    {
        public int SubjectID { get; set; }

        [Required(ErrorMessage = "El nombre de la materia es requerido")]
        [StringLength(100, ErrorMessage = "El nombre de la materia no puede exceder los 100 caracteres")]
        public string SubjectName { get; set; }

        [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10")]
        public int Credits { get; set; } = 3;

        public int ProfessorID { get; set; }

        public bool State { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
