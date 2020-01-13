using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCourses.Data;

namespace WebCourses.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Students/Details/5
        [Route("/Courses/{courseId}/Students/Details/{studentId}")]
        public async Task<IActionResult> Details(string courseId, string studentId)
        {
            if (courseId == null || studentId == null)
            {
                return NotFound();
            }

            // podejrzewam że będzie trzeba zrobić viewmodel z emailem użytkownika i kursem includując w _dbContext test, by móc wyciągnąć wyniki studenta dla każdego testu

            //var test = await _context.Tests
            //    .Include(t => t.Course)
            //    .FirstOrDefaultAsync(t => t.Id == testId && t.CourseId == courseId);
            //if (test == null)
            //{
            //    return NotFound();
            //}

            return View(/*student*/);
        }
    }
}