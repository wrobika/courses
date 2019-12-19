using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class User
    {
        public string UserIdentityId { get; set; } //klucz zewnętrzny z IdentityUser
        public IdentityUser UserIdentity { get; set; }
        
        public List<Course> Courses { get; set; }
        public List<Test> Tests { get; set; }

    }
}
