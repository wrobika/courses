using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IList<CourseUser> CourseUsers { get; set; }

        public string UserId { get; set; }

        [DisplayName("Teacher")]
        public User User { get; set; }

        public IList<Test> Tests { get; set; }
    }
}
