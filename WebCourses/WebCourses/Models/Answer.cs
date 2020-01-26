using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebCourses.Models
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string QuestionId { get; set; }

        public Question Question { get; set; }

        [Required]
        [StringLength(100)]
        public string Content { get; set; }

        public Boolean Correct { get; set; }
    }
}
