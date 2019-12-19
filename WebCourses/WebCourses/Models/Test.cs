using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class Test
    {
        public string Id { get; set; }

        public string Title { get; set; }           //nazwa testu

        public Course FromCourse { get; set; }      //kurs z którego jest test
        public string FromCourseId { get; set; }    //klucz zewnetrzny kursu

        public List<ClosedQuestion> ClosedQuestions { get; set; }   //lista zamknietych pytan z testu
        public List<OpenQuestion> OpenQuestions { get; set; }       //lista otwartych pytan z testu

        public List<Result> Results { get; set; }   //wyniki wszystkich studentow podchodzacych do testu
    }
}
