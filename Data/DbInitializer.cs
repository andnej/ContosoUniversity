using ContosoUniversity.Models;
using System;
using System.Linq;

namespace ContosoUniversity.Data 
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();
            if (context.Students.Any()) {
                return;
            }

            var students = new Student[] 
            {
                new Student{FirstMidName="Bjorn", LastName="Stroussop", EnrollmentDate=DateTime.Parse("2010-05-05")},
                new Student{FirstMidName="Charles", LastName="Champagne", EnrollmentDate=DateTime.Parse("2010-05-05")},
                new Student{FirstMidName="Dinah", LastName="Drake", EnrollmentDate=DateTime.Parse("2010-05-05")},
                new Student{FirstMidName="Edna", LastName="Eigner", EnrollmentDate=DateTime.Parse("2010-05-05")},
                new Student{FirstMidName="Frank", LastName="Foster", EnrollmentDate=DateTime.Parse("2010-05-05")},
                new Student{FirstMidName="Grant", LastName="Gustin", EnrollmentDate=DateTime.Parse("2010-05-05")},
                new Student{FirstMidName="Harry", LastName="Hallowell", EnrollmentDate=DateTime.Parse("2010-05-05")},
            };
            foreach(Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            var courses = new Course[] {
                new Course{CourseID=1001, Title="Algebra", Credits=3},
                new Course{CourseID=1002, Title="Trigonometri", Credits=3},
                new Course{CourseID=1003, Title="Chemistry", Credits=3},
                new Course{CourseID=1004, Title="Economics", Credits=3}
            };
            foreach (Course c in courses)
            {
               context.Courses.Add(c);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[] {
                new Enrollment{StudentID=1, CourseID=1001, Grade=Grade.A},
                new Enrollment{StudentID=2, CourseID=1002, Grade=Grade.A},
                new Enrollment{StudentID=3, CourseID=1003},
                new Enrollment{StudentID=1, CourseID=1001, Grade=Grade.A},
                new Enrollment{StudentID=2, CourseID=1002, Grade=Grade.A},
                new Enrollment{StudentID=3, CourseID=1003},
            };
            foreach(Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();
        }
    }
}