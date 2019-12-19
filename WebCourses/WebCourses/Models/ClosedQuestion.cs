using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class ClosedQuestion
    {
        public string Id { get; set; }

        public Test FromTest { get; set; }                  //test z którego jest pytanie
        public string FromTestId { get; set; }              //klucz zewnetrzny testu

        public string Content { get; set; }                 //tresc pytania
        public List<string> AllAnswers{ get; set; }         //lista odpowiedzi wszystkich
        public List<string> CorrectAnswers { get; set; }    //lista odpowiedzi dobrych

    }
}
