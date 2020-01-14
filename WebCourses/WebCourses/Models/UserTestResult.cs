using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class UserTestResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string UserId { get; set; }       //id uzytkownika odpowiadajacego
        public User User { get; set; }

        public string TestId { get; set; }       //id testu
        public Test Test { get; set; }

        public int PointsCount { get; set; }     //liczba punktow za test
        public Boolean Checked { get; set; }     //czy test zostal calkowicie sprawdzony (nauczyciel sprawdzil pytania otwarte)

    }
}
