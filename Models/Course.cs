using System;
using System.Collections.Generic;

namespace ContosoUniversity.Models
{
    public class Course
    {
        public int CourseID { get;set; }
        public string Title { get;set; }
        public Int32 Credits { get;set; }

        public ICollection<Enrollment> Enrollments { get;set; }
    }
}