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

namespace WebCourses.Controllers.Courses
{
    public class TestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tests
        [Route("/Courses/{courseId}/Tests")]
        public async Task<IActionResult> Index(string courseId)
        {
            var courseTests = await _context.Tests
                .Where(t => t.CourseId == courseId)
                .ToListAsync();
            return View(courseTests);
        }

        // GET: Tests/Details/5
        [Route("/Courses/{courseId}/Tests/Details/{testId}")]
        public async Task<IActionResult> Details(string courseId, string testId)
        {
            if (courseId == null || testId == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .Include(t => t.Course)
                .FirstOrDefaultAsync(t => t.Id == testId && t.CourseId == courseId);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // GET: Tests/Create
        [Route("/Courses/{courseId}/Tests/Create")]
        public IActionResult Create(string courseId)
        {
            var course = _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            ViewData["CourseId"] = courseId;
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Courses/{courseId}/Tests/Create")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ReleaseDate,Deadline,CourseId")] Test test, string courseId)
        {
            if (ModelState.IsValid)
            {
                _context.Tests.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { courseId = courseId, testId = test.Id });
            }
            ViewData["CourseId"] = courseId;
            return View(test);
        }

        // GET: Tests/Edit/5
        [Route("/Courses/{courseId}/Tests/Edit/{testId}")]
        public async Task<IActionResult> Edit(string courseId, string testId)
        {
            if (courseId == null || testId == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .FirstAsync(t => t.Id == testId);
            if (test == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", test.CourseId);
            return View(test);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Courses/{courseId}/Tests/Edit/{testId}")]
        public async Task<IActionResult> Edit(string testId, [Bind("Id,Name,Description,ReleaseDate,Deadline,CourseId")] Test test, string courseId)
        {
            if (testId != test.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", test.CourseId);
            return View(test);
        }

        // GET: Tests/Delete/5
        [Route("/Courses/{courseId}/Tests/Delete/{testId}")]
        public async Task<IActionResult> Delete(string courseId, string testId)
        {
            if (testId == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .Include(t => t.Course)
                .FirstOrDefaultAsync(m => m.Id == testId);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var test = await _context.Tests.FindAsync(id);
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(string id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }

        // GET: Tests/Solve/5
        [Route("/Courses/{courseId}/Tests/Solve/{testId}")]
        public async Task<IActionResult> Solve(string courseId, string testId)
        {
            if (courseId == null || testId == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .FirstAsync(t => t.Id == testId);
            if (test == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", test.CourseId);
            TestViewModel testViewModel = new TestViewModel
            {
                TestId = test.Id,
                CourseId = test.CourseId,
                Questions = new List<QuestionViewModel>()
            };
            foreach(var question in test.Questions)
            {
                QuestionViewModel questionViewModel = new QuestionViewModel
                {
                    QuestionId = question.Id,
                    TestId = question.TestId,
                    Content = question.Content,
                    Type = question.Type,
                    Answers = new List<AnswerViewModel>()
                };
                foreach(var answer in question.Answers)
                {
                    AnswerViewModel answerViewModel = new AnswerViewModel
                    {
                        AnswerId = answer.Id,
                        Content = answer.Content,
                        Selected = false,
                        Correct = answer.Correct
                    };
                    questionViewModel.Answers.Add(answerViewModel);
                }
                testViewModel.Questions.Add(questionViewModel);
            }
            return View(testViewModel);
        }

        public async Task<int> Check([Bind("TestId,CourseId,Questions")] TestViewModel testViewModel, string courseId, string testId)
        {
            int countCorrect = 0;
            foreach (var question in testViewModel.Questions)
            {
                switch(question.Type)
                {
                    case Question.QuestionType.SingleAnswer:
                        int i = 0;
                        while (!question.Answers[i].Correct) i++;
                        if (question.SelectedAnswerId == question.Answers[i].AnswerId)
                            countCorrect++;
                        break;
                    case Question.QuestionType.MultipleAnswer:
                        foreach (var answer in question.Answers)
                        {
                            if (answer.Correct && answer.Selected)
                                countCorrect++;
                        }
                        break;
                    case Question.QuestionType.Open:
                        break;
                    default: break;
                }
            }
            return countCorrect;
        }
    }
}
