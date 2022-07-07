
namespace P01_StudentSystem.Data.Models
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public Student()
        {
            this.CourseEnrollments = new HashSet<StudentCourse>();
            this.HomeworkSubmissions = new HashSet<Homework>();
        }
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.StudentNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(GlobalConstants.PhoneNumberNameMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public virtual ICollection<StudentCourse> CourseEnrollments { get; set; }
        public virtual ICollection<Homework> HomeworkSubmissions { get; set; }
    }
}
