using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCourses.Data;
using WebCourses.Models;

namespace WebCourses.Controllers
{
    public class OpenQuestionAnswersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public OpenQuestionAnswersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: OpenQuestionAnswers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OpenQuestionAnswers.Include(o => o.Question).Include(o => o.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OpenQuestionAnswers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openQuestionAnswer = await _context.OpenQuestionAnswers
                .Include(o => o.Question)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (openQuestionAnswer == null)
            {
                return NotFound();
            }

            return View(openQuestionAnswer);
        }

        // GET: OpenQuestionAnswers/Create
        public IActionResult Create()
        {
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: OpenQuestionAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,UserId,QuestionId,Checked")] OpenQuestionAnswer openQuestionAnswer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(openQuestionAnswer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", openQuestionAnswer.QuestionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", openQuestionAnswer.UserId);
            return View(openQuestionAnswer);
        }

        // GET: OpenQuestionAnswers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openQuestionAnswer = await _context.OpenQuestionAnswers.FindAsync(id);
            if (openQuestionAnswer == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", openQuestionAnswer.QuestionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", openQuestionAnswer.UserId);
            return View(openQuestionAnswer);
        }

        // POST: OpenQuestionAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Content,UserId,QuestionId,Checked")] OpenQuestionAnswer openQuestionAnswer)
        {
            if (id != openQuestionAnswer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(openQuestionAnswer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpenQuestionAnswerExists(openQuestionAnswer.Id))
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
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", openQuestionAnswer.QuestionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", openQuestionAnswer.UserId);
            return View(openQuestionAnswer);
        }

        // GET: OpenQuestionAnswers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openQuestionAnswer = await _context.OpenQuestionAnswers
                .Include(o => o.Question)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (openQuestionAnswer == null)
            {
                return NotFound();
            }

            return View(openQuestionAnswer);
        }

        // POST: OpenQuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var openQuestionAnswer = await _context.OpenQuestionAnswers.FindAsync(id);
            _context.OpenQuestionAnswers.Remove(openQuestionAnswer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MarkCorrect(string courseId, string testId, string answerId)
        {
            User currentUser = await _userManager.GetUserAsync(User);
            var answer = await _context.OpenQuestionAnswers
                //.Include(a => a.Question)
                .FirstOrDefaultAsync(a => a.Id == answerId);
            //var testId = answer.Question.TestId;
            var testResult = await _context.UserTestResults
                .FirstOrDefaultAsync(utr => utr.TestId == testId && utr.UserId == currentUser.Id);
            testResult.PointsCount++;
            UserTestResultsController resultsController = new UserTestResultsController(_context);
            await resultsController.Edit(testResult.Id, testResult);

            answer.Checked = true;
            await Edit(answer.Id, answer);
            return RedirectToAction("Details", "Tests", new { courseId, testId });
        }

        public async Task<IActionResult> MarkIncorrect(string courseId, string testId, string answerId)
        {
            var answer = await _context.OpenQuestionAnswers
                .FirstOrDefaultAsync(a => a.Id == answerId);
            answer.Checked = true;
            await Edit(answer.Id, answer);
            return RedirectToAction("Details", "Tests", new { courseId, testId });
        }

        private bool OpenQuestionAnswerExists(string id)
        {
            return _context.OpenQuestionAnswers.Any(e => e.Id == id);
        }
    }
}
