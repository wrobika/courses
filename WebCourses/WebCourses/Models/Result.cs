using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class Result
    {
        public string Id { get; set; }

        public User FromUser { get; set; }                  //wynik nalezy do konkretnego studenta
        public string FromUserId { get; set; }              //klucz zewnetrzny student

        public Test FromTest { get; set; }                  //test z którego jest wynik
        public string FromTestId { get; set; }              //klucz zewnetrzny testu

        public int GoodAnswers { get; set; }                //ilosc odp dobrych
        public int BadAnswers { get; set; }                 //ilosc odp zlych

    }
}
