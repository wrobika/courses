using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCourses.Data;
using WebCourses.Models;

namespace WebCourses.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CoursesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = new List<Course>();
            User currentUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(currentUser, "Teacher"))
            {
                courses = await _context.Courses.Where(c => c.UserId == currentUser.Id).ToListAsync();
            }
            else if (await _userManager.IsInRoleAsync(currentUser, "Student"))
            {
                courses = await _context.CourseUsers
                    .Include(cu => cu.Course)
                    .Where(cu => cu.UserId == currentUser.Id)
                    .Where(cu => cu.Confirmed)
                    .Select(cu => cu.Course).ToListAsync();
            }
            else
            {
                return Challenge();
            }
            return View(courses);
        }

        // GET: /Courses/Apply
        public async Task<IActionResult> Apply()
        {
            User currentUser = await _userManager.GetUserAsync(User);
            var userCourseIds = await _context.CourseUsers
                .Where(cu => cu.UserId == currentUser.Id)
                .Select(cu => cu.CourseId).ToListAsync();

            var courses = await _context.Courses
                .Include(c => c.User)
                .Where(c => !userCourseIds.Contains(c.Id)).ToListAsync();

            return View(courses);
        }

        [HttpPost]
        // POST: /Courses/Register
        public async Task<IActionResult> Register(string courseId)
        {
            User currentUser = await _userManager.GetUserAsync(User);
            var courseUser = new CourseUser()
            {
                UserId = currentUser.Id,
                CourseId = courseId,
            };
            _context.CourseUsers.Add(courseUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Courses");
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(string confirmation, string userId, string courseId) {
            if (confirmation == "true") {
                var courseUser = await _context.CourseUsers
                     .Where(cu => cu.UserId == userId)
                     .Where(cu => cu.CourseId == courseId)
                     .FirstOrDefaultAsync();
                courseUser.Confirmed = true;
                _context.CourseUsers.Update(courseUser);
                await _context.SaveChangesAsync();
            }

            if (confirmation == "false") {
                var courseUser = await _context.CourseUsers
                    .Where(cu => cu.UserId == userId)
                    .Where(cu => cu.CourseId == courseId)
                    .FirstOrDefaultAsync();
                _context.CourseUsers.Remove(courseUser);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Courses", new {id = courseId });
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.CourseUsers)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Tests)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(string id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
