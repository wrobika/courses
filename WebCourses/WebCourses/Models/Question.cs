using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class Question
    {
        public enum QuestionType
        {
            SingleAnswer,
            MultipleAnswer,
            Open,
            Categorization
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string TestId { get; set; }

        public Test Test { get; set; }

        public string Content { get; set; }

        public QuestionType Type { get; set; }

        [DefaultValue(0)]
        public int AnswersCount { get; set; }

        [DefaultValue(0)]
        public int CorrectAnswersCount { get; set; }

        public IList<Answer> Answers { get; set; }
    }
}
