namespace UniversityAutentication.Models
{
    public class Course
    {

        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int SeatCapacity { get; set; }
        public virtual Instructor? Instructor { get; set; }
        // En la parte del 1 a muchos poner el virutl en el 1
    }
}
