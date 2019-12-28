using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class User : IdentityUser
    {   
        public IList<CourseUser> CourseUsers { get; set; }

        public IList<Course> Courses { get; set; }
    }
}
