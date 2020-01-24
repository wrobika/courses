using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var student = await _context.Users
                .FirstOrDefaultAsync(user => user.Id == studentId);
            var userTestResults = _context.UserTestResults
                .Include(result => result.Test)
                    .ThenInclude(test => test.Questions)
                .Where(result => result.Test.CourseId == courseId && result.UserId == studentId);
            ViewBag.courseId = courseId;
            ViewBag.userTestResults = userTestResults;
            return View(student);
        }
    }
}