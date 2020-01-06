using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCourses.Data;
using WebCourses.Models;
using WebCourses.ViewModels;

namespace WebCourses.Controllers.Courses.Tests
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Questions
        [Route("/Courses/{courseId}/Tests/{testId}/Questions")]
        public async Task<IActionResult> Index(string courseId, string testId)
        {
            var questionTypes = new List<SelectListItem>();
            foreach (Question.QuestionType type in Enum.GetValues(typeof(Question.QuestionType)))
            {
                questionTypes.Add(new SelectListItem {
                    Text = Enum.GetName(typeof(Question.QuestionType), type), Value = type.ToString()
                });
            }
            ViewBag.QuestionTypes = questionTypes;
            return View();
        }

        [Route("/Courses/{courseId}/Tests/{testId}/Questions/Choose")]
        public async Task<IActionResult> Choose(ChooseQuestionTypeViewModel questionTypeModel, string courseId, string testId)
        {
            ViewBag.Choice = questionTypeModel.QuestionType;
            return View("Create");
        }

            // GET: Questions/Details/5
            public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Test)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id");
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TestId,Content,Type,AnswersCount,CorrectAnswersCount")] Question question, string courseId, string testId)
        {
            if (ModelState.IsValid)
            {
                question.TestId = testId;
                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { courseId = courseId, testId = testId, questionId = question.Id});
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", question.TestId);
            return View(question);
        }

        // GET: Questions/Edit/5
        [Route("/Courses/{courseId}/Tests/{testId}/Questions/Edit/{questionId}")]
        public async Task<IActionResult> Edit(string courseId, string testId, string questionId)
        {
            if (questionId == null || testId == null || courseId == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstAsync(q => q.Id == questionId);

            if (question == null)
            {
                return NotFound();
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", question.TestId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TestId,Content,Type,AnswersCount,CorrectAnswersCount")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
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
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", question.TestId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Test)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var question = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(string id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
