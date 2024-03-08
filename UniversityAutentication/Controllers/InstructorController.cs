using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAutentication.Data;
using UniversityAutentication.Models;

namespace UniversityAutentication.Controllers
{
    [Authorize(Roles = "Instructor,Admin")]

    public class InstructorController : Controller
    {

        private readonly ApplicationDbContext _db;

        public InstructorController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task <IActionResult> Index()
        {
            ViewData["Registrado"]=false;
            if(User.Identity.Name != null)
            {
                //Esta en la tabla de usuarios 

                //ahora miramos si esta tambien en la de instructores 

                var instructor = await _db.Instructors.FirstOrDefaultAsync(i => i.InstructorUser == User.Identity.Name);
                if(instructor != null)
                {

                    //Existe en la de instructors tambien 
                    ViewBag.Instructor = instructor.InstructorId;
                    ViewData["Registrado"] = true;


                }


            }


            return View();
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> AllProfiles()
        {

            var instructores = await _db.Instructors.ToListAsync();
            return View(instructores);  
        }

        public IActionResult AddProfile()
        {

            return View();

        }



        [HttpPost]

        public async Task <IActionResult> AddProfile( Instructor  instructor)
        {

            _db.Add(instructor);
            await _db.SaveChangesAsync();
            return RedirectToAction("AllProfiles");
        }



        public async Task<IActionResult> EditProfile(int id)
        {
            var instructorToUpdate=await _db.Instructors.FirstOrDefaultAsync(i=>i.InstructorId == id);

            return View(instructorToUpdate);
        }


        [HttpPost]

        public async Task <IActionResult> EditProfile(Instructor instructor)
        {
            _db.Update(instructor);
            await _db.SaveChangesAsync();
            return RedirectToAction("AllProfiles");
        }

    }
}
