using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebCourses.Models;

namespace WebCourses.ViewModels
{
    public class TestViewModel
    {
        [Key]
        public string TestId { get; set; }
        public string CourseId { get; set; }

        [BindProperty]
        public IList<QuestionViewModel> Questions { get; set; }
    }
}
