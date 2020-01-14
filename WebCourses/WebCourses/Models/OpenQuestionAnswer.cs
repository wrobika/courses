using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class OpenQuestionAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Content { get; set; } //odpowiedz uzytkownika

        public string UserId { get; set; }      //id uzytkownika odpowiadajacego
        public User User { get; set; }

        public string QuestionId { get; set; }      //id pytania otwartego
        public Question Question { get; set; }

        public Boolean Checked { get; set; }      //czy sprawdzone przez nauczyciela

    }
}
