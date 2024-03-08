using System.ComponentModel.DataAnnotations;

namespace UniversityAutentication.Models
{
    public class Enrollment
    {

        public int EnrollmentId { get; set; }
        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }

        [DisplayFormat(NullDisplayText ="No tiene grado")]
        public LetterGrade? LetterGrade { get; set; }


    }

    public enum LetterGrade {

        A, B, C, D,F,I,W,P
    }
}
