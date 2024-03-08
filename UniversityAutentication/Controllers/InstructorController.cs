using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityAutentication.Data;

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

        public IActionResult Index()
        {
            return View();
        }
    }
}
