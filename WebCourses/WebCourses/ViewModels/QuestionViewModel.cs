using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static WebCourses.Models.Question;

namespace WebCourses.ViewModels
{
    public class QuestionViewModel
    {
        [Key]
        public string QuestionId { get; set; }

        public string Content { get; set; }

        public QuestionType Type { get; set; }

        [BindProperty]
        public IList<AnswerViewModel> Answers { get; set; }

        public string SelectedAnswerId { get; set; }

        public string OpenAnswer { get; set; }
    }
}
