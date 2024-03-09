using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityAutentication.Data;
using UniversityAutentication.Models;
using UniversityAutentication.ViewModels;

namespace UniversityAutentication.Controllers
{
    [Authorize(Roles = "Student")]

    public class StudentController : Controller
    {


        private readonly ApplicationDbContext _db;

        public StudentController(ApplicationDbContext db)
        {
            _db = db;
        }



        public async Task<IActionResult> Index()
        {
            ViewData["Registrado"] = false;
            if (User.Identity.Name != null)
            {
                //Esta en la tabla de usuarios 

                //ahora miramos si esta tambien en la de instructores 

                var student = await _db.Students.FirstOrDefaultAsync(i => i.StudentUser == User.Identity.Name);
                if (student != null)
                {

                    //Existe en la de instructors tambien 
                    ViewBag.StudentId = student.StudentId;
                    ViewData["Registrado"] = true;


                }


            }
            return View();
        }

        [Authorize(Roles = "Student")]
        public IActionResult AddProfile()
        {

            var currentUserId = User.Identity.Name;
            Student student = new Student();
            student.StudentUser=currentUserId;

            return View(student);

        }



        [HttpPost]

        public async Task<IActionResult> AddProfile(Student student)
        {

            _db.Add(student);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EditProfile(int id)
        {
            var instructorToUpdate = await _db.Students.FirstOrDefaultAsync(i => i.StudentId == id);

            return View(instructorToUpdate);
        }


        [HttpPost]

        public async Task<IActionResult> EditProfile(Student student)
        {
            _db.Update(student);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> EnrollCourse()
        {

            var currentUserId = User.Identity.Name;

            Student studentToShow = await _db.Students.Where(s=>s.StudentUser == currentUserId).FirstOrDefaultAsync();


            var courseDisplay = await _db.Courses.Select(x=> new
            {

                Id=x.CourseId,
                Value=x.CourseTitle


            }).ToListAsync();

            StudentAddEnrollmentViewModels vm = new StudentAddEnrollmentViewModels();

            vm.CourseList = new SelectList(courseDisplay, "Id", "Value");

            return View(vm);

        }


        [HttpPost]

        public async Task<IActionResult> EnrollCourse(StudentAddEnrollmentViewModels vm)
        {
            //Curso
            Course? course = _db.Courses.
                Where(c => c.CourseId == vm.Course.CourseId).
                Include(c => c.Instructor).FirstOrDefault();

            //Student
            Student? student = _db.Students.
                Where(s => s.StudentId == vm.Student.StudentId).FirstOrDefault();


            //Comprobar si ya matriculado
            Enrollment? yaMatriculado = _db.Enrollments.
                Where(e => e.Student == student && e.Course == course).FirstOrDefault();
            if (yaMatriculado != null)
            {
                //Mostrar una vista de ya matriculado
            }
            else
            {
                if (course.SeatCapacity > 0)
                {
                    Enrollment enrollment = new Enrollment();

                    enrollment.Course = course;
                    enrollment.Student = student;
                    _db.Add(enrollment);

                    //decrementamos el numero de plazas
                    course.SeatCapacity--;
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //Mostrar un div con el numero de plazas
                }
            }
            return RedirectToAction("Index");

            //Comprobar si hay plazas, si hay matricular y descontar el numero

            //Redireccionar al Index
        }







        public int DevuelveCapacidad (int dato)
        {

         Course? course = _db.Courses.Where(c=>c.CourseId == dato).FirstOrDefault();

            
            if(course != null)
            {

                return course.SeatCapacity;


            }
            else
            {
                return 0;
            }




        }







    }
}


