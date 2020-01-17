using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.ViewModels
{
    public class AnswerViewModel
    {
        [Key]
        public string AnswerId { get; set; }
        public string Content { get; set; }
        public bool Selected { get; set; }
        public bool Correct { get; set; }
    }
}
