using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCourses.Data;
using WebCourses.Models;

namespace WebCourses.Controllers.Courses.Tests.Questions
{
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnswersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Answers.Include(a => a.Question);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        [Route("/Courses/{courseId}/Tests/{testId}/Questions/{questionId}/Answers/Create")]
        public async Task<IActionResult> Create(string courseId, string testId, string questionId)
        {
            var questionType = await _context.Questions
                .Where(q => q.Id == questionId)
                .Select(q => q.Type).FirstAsync();
            var correctAnswerCount = await _context.Answers
                .Where(a => a.QuestionId == questionId)
                .CountAsync(a => a.Correct == true);
            ViewBag.QuestionType = questionType;
            ViewBag.CorrectAnswerCount = correctAnswerCount;
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Courses/{courseId}/Tests/{testId}/Questions/{questionId}/Answers/Create")]
        public async Task<IActionResult> Create([Bind("Id,QuestionId,Content,Correct")] Answer answer, string courseId, string testId, string questionId)
        {
            if (ModelState.IsValid)
            {
                var question = await _context.Questions.FindAsync(questionId);
                _context.Answers.Add(answer);
                question.AnswersCount++;
                if (answer.Correct) {
                    question.CorrectAnswersCount++;
                }
                _context.Questions.Update(question);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Questions", new { courseId = courseId, testId = testId, questionId = questionId });
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", answer.QuestionId);
            return View(answer);
        }

        // GET: Answers/Edit/5
        [Route("/Courses/{courseId}/Tests/{testId}/Questions/{questionId}/Edit/{answerId}")]
        public async Task<IActionResult> Edit(string courseId, string testId, string questionId, string answerId)
        {
            if (answerId == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.FindAsync(answerId);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", answer.QuestionId);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,QuestionId,Content,Correct")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
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
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", answer.QuestionId);
            return View(answer);
        }

        // GET: Answers/Delete/5
        [Route("/Courses/{courseId}/Tests/{testId}/Questions/{questionId}/Answers/Delete/{answerId}")]
        public async Task<IActionResult> Delete(string answerId)
        {
            if (answerId == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == answerId);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var answer = await _context.Answers.FindAsync(id);
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(string id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }
    }
}
