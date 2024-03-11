using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityAutentication.Data;
using UniversityAutentication.Models;
using UniversityAutentication.ViewModels;

namespace UniversityAuthentication.Controllers
{
    [Authorize(Roles = "Instructor, Admin")]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _db;
        public InstructorController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Registrado"] = false;

            if (User.Identity.Name != null)
            {
                //Esta en la tabla de usuarios
                //Comprobamos si esta en la tabla de Instructores
                var instructor = await _db.Instructors.FirstOrDefaultAsync(i => i.InstructorUser == User.Identity.Name);
                if (instructor != null)
                {
                    //Existe este instructor
                    ViewBag.InstructorId = instructor.InstructorId;
                    ViewData["Registrado"] = true;
                }
            }

            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllProfiles()
        {
            var instructores = await _db.Instructors.ToListAsync();
            return View(instructores);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProfile(Instructor instructor)
        {
            _db.Add(instructor);
            await _db.SaveChangesAsync();
            return RedirectToAction("AllProfiles");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProfile(int id)
        {
            var instructorToUpadate = await _db.Instructors.FirstOrDefaultAsync(i => i.InstructorId == id);
            return View(instructorToUpadate);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(Instructor instructor)
        {
            _db.Update(instructor);
            await _db.SaveChangesAsync();
            return RedirectToAction("AllProfiles");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCourse()
        {
            var instructorDisplay = await _db.Instructors.Select(x => new
            {
                Id = x.InstructorId,
                Value = x.InstructorName
            }).ToListAsync();
            InstructorAddCourseViewModel vm = new InstructorAddCourseViewModel();
            vm.InstructorSelecList = new SelectList(instructorDisplay, "Id", "Value");
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddCourse(InstructorAddCourseViewModel vm)
        {
            var instructor = await _db.Instructors.FirstOrDefaultAsync(i => i.InstructorId == vm.Instructor.InstructorId);
            vm.Course.Instructor = instructor;
            _db.Add(vm.Course);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Instructor");
        }

        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GradeCourse()
        {
            var currentUserId = User.Identity.Name;
            Instructor instructorToShow = await _db.Instructors
                .Where(i => i.InstructorUser == currentUserId).FirstOrDefaultAsync();
            if (instructorToShow != null)
            {
                List<Course> listaCursosInstructor = await _db.Courses
                    .Where(c => c.Instructor.InstructorId == instructorToShow.InstructorId).ToListAsync();
                var cursoDisplay = listaCursosInstructor.Select(x => new
                {
                    Id = x.CourseId,
                    Value = x.CourseTitle
                }).ToList();
                InstructorGradeCourseViewModel vm = new InstructorGradeCourseViewModel();
                vm.CourseList = new SelectList(cursoDisplay, "Id", "Value");
                vm.Instructor = instructorToShow;
                return View(vm);
            }
            else
            {
                return View();
            }
        }



        public async Task<List<Enrollment>> DevuelveMatriculas(int dato)
        {

            List<Enrollment> Matriculados = await _db.Enrollments.Include(e => e.Student).Where(e => e.Course.CourseId == dato).ToListAsync();

            return Matriculados;
        }
        public async Task<IActionResult> Califica(int dato, char nota)
        {
            Enrollment enrollment = await _db.Enrollments.Where(e => e.Student.StudentId == dato).FirstOrDefaultAsync();

            if (enrollment != null)
            {
                enrollment.LetterGrade = (LetterGrade)nota;
                _db.Update(enrollment);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Instructor");
        }
    }
}



