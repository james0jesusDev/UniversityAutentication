using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityAutentication.Models;

namespace UniversityAutentication.ViewModels
{
    public class InstructorAddCourseViewModel
    {

        public Course? Course { get; set; }
        public Instructor? Instructor { get; set; }
        public SelectList? InstructorSelecList { get; set; }

    }
}
