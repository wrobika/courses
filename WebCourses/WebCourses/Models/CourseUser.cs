using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class CourseUser
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string CourseId { get; set; }
        public Course Course { get; set; }

        [DefaultValue(false)]
        public Boolean Confirmed { get; set; } = false;

    }
}
