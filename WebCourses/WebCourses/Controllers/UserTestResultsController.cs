using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCourses.Data;
using WebCourses.Models;

namespace WebCourses.Controllers
{
    public class UserTestResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserTestResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserTestResults
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserTestResults.Include(u => u.Test).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserTestResults/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTestResult = await _context.UserTestResults
                .Include(u => u.Test)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userTestResult == null)
            {
                return NotFound();
            }

            return View(userTestResult);
        }

        // GET: UserTestResults/Create
        public IActionResult Create()
        {
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserTestResults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TestId,PointsCount,Checked")] UserTestResult userTestResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTestResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", userTestResult.TestId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userTestResult.UserId);
            return View(userTestResult);
        }

        // GET: UserTestResults/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTestResult = await _context.UserTestResults.FindAsync(id);
            if (userTestResult == null)
            {
                return NotFound();
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", userTestResult.TestId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userTestResult.UserId);
            return View(userTestResult);
        }

        // POST: UserTestResults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserId,TestId,PointsCount,Checked")] UserTestResult userTestResult)
        {
            if (id != userTestResult.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTestResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTestResultExists(userTestResult.Id))
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
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", userTestResult.TestId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userTestResult.UserId);
            return View(userTestResult);
        }

        // GET: UserTestResults/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTestResult = await _context.UserTestResults
                .Include(u => u.Test)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userTestResult == null)
            {
                return NotFound();
            }

            return View(userTestResult);
        }

        // POST: UserTestResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userTestResult = await _context.UserTestResults.FindAsync(id);
            _context.UserTestResults.Remove(userTestResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTestResultExists(string id)
        {
            return _context.UserTestResults.Any(e => e.Id == id);
        }
    }
}
