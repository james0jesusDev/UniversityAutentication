using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityAutentication.Models;

namespace UniversityAutentication.ViewModels
{
    public class InstructorGradeCourseViewModel
    {

        public Course? Course { get; set; }
        public Instructor? Instructor { get; set; }
        public SelectList? CourseList { get; set; }


    }
}
