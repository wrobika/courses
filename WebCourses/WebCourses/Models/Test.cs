using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class Test
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DisplayName("Release Date")]
        public DateTime? ReleaseDate { get; set; } //data udostepnienia

        [DisplayName("Deadline Date")]
        public DateTime? Deadline { get; set; } //data deadlinu

        public string CourseId { get; set; }

        public Course Course { get; set; }

        public IList<Question> Questions { get; set; }

        public IList<UserTestResult> UserTestResults { get; set; }
    }
}
