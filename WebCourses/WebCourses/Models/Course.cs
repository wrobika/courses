using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class Course
    {
        public string Id { get; set; }

        public string Title { get; set; }           //nazwa kursu

        public User Teacher { get; set; }           //nauczyciel tworzacy kurs
        public string TeacherId { get; set; }       //klucz obcy nauczyciela

        public List<User> Students { get; set; }    //uczniowie z dostepem do kursu
        public List<Test> Tests { get; set; }       // testy w kursie

    }
}
