using System;
using System.Linq;
using DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace DataAccess.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(ILogger<DbInitializer> logger)
        {
            _logger = logger;
        }

        public void Initialize(EducationDbContext context)
        {
            _logger.LogInformation("Start seeding test database");

            var students = new StudentDb[]
            {
                new StudentDb { FirstName = "Janet", LastName = "Gates", Email = "janet1@education.com", PhoneNumber = "710-555-0173" },
                new StudentDb { FirstName = "Lucy", LastName = "Harrington", Email = "lucy0@education.com", PhoneNumber = "828-555-0186" },
                new StudentDb { FirstName = "Kathleen", LastName = "Garza", Email = "kathleen0@education.com", PhoneNumber = "150-555-0127" },
                new StudentDb { FirstName = "Katherine", LastName = "Harding", Email = "katherine0@education.com", PhoneNumber = "926-555-0159" },
                new StudentDb { FirstName = "Johnny", LastName = "Caprio", Email = "johnny0@education.com", PhoneNumber = "112-555-0191" },

                new StudentDb { FirstName = "Christopher", LastName = "Beck", Email = "christoph1@education.com", PhoneNumber = "216-555-0122" },

                // new StudentDb { FirstName = "David", LastName = "Liu", Email = "david20@education.com", PhoneNumber = "440-555-0132" },
                // new StudentDb { FirstName = "Jinghao", LastName = "Liu", Email = "jinghao1@education.com", PhoneNumber = "928-555-0116" },
                // new StudentDb { FirstName = "Linda", LastName = "Burnett", Email = "linda4@education.com", PhoneNumber = "216-444-0122" },
                // new StudentDb { FirstName = "Kerim", LastName = "Hanif", Email = "kerim0@education.com", PhoneNumber = "216-555-0122" },
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            var teachers = new TeacherDb[]
            {
                new TeacherDb { FirstName = "Sharon", LastName = "Looney", Email = "sharon2@knowledge.com", PhoneNumber = "377-555-0132" },
                new TeacherDb { FirstName = "Darren", LastName = "Gehring", Email = "darren0@knowledge.com", PhoneNumber = "417-555-0182" },

                new TeacherDb { FirstName = "Erin", LastName = "Hagens", Email = "erin1@knowledge.com", PhoneNumber = "244-555-0127" },

                // new TeacherDb { FirstName = "Jeremy", LastName = "Los", Email = "jeremy0@knowledge.com", PhoneNumber = "911-555-0165" },
                // new TeacherDb { FirstName = "Elsa", LastName = "Leavitt", Email = "elsa0@knowledge.com", PhoneNumber = "482-555-0174" }
            };

            context.Teachers.AddRange(teachers);
            context.SaveChanges();

            var lectures = new LectureDb[]
            {
                new LectureDb { Topic = "Architecture", Date = new DateTime(2022, 1, 5, 18, 0, 0), TeacherId = 1 },
                new LectureDb { Topic = "Climate", Date = new DateTime(2022, 1, 12, 18, 0, 0), TeacherId = 1 },
                new LectureDb { Topic = "Fatigue", Date = new DateTime(2022, 1, 19, 18, 0, 0), TeacherId = 2 },
                new LectureDb { Topic = "Structures", Date = new DateTime(2022, 1, 26, 18, 0, 0), TeacherId = 2 },
                new LectureDb { Topic = "Hydraulics", Date = new DateTime(2022, 2, 2, 18, 0, 0), TeacherId = 3 },

                // new LectureDb { Topic = "Construction", Date = new DateTime(2022, 02, 9, 18, 0, 0) }
            };

            context.Lectures.AddRange(lectures);
            context.SaveChanges();

            var attendances = new AttendanceDb[]
            {
                new AttendanceDb { LectureId = 1, StudentId = 1, IsAttended = true, HomeworkMark = 5 },
                new AttendanceDb { LectureId = 1, StudentId = 2, IsAttended = true, HomeworkMark = 4 },
                new AttendanceDb { LectureId = 1, StudentId = 3, IsAttended = true, HomeworkMark = 3 },
                new AttendanceDb { LectureId = 1, StudentId = 4, IsAttended = true, HomeworkMark = 2 },
                new AttendanceDb { LectureId = 1, StudentId = 5, IsAttended = false, HomeworkMark = 0 },

                new AttendanceDb { LectureId = 2, StudentId = 1, IsAttended = true, HomeworkMark = 4 },
                new AttendanceDb { LectureId = 2, StudentId = 2, IsAttended = true, HomeworkMark = 4 },
                new AttendanceDb { LectureId = 2, StudentId = 3, IsAttended = true, HomeworkMark = 4 },
                new AttendanceDb { LectureId = 2, StudentId = 4, IsAttended = false, HomeworkMark = 0 },
                new AttendanceDb { LectureId = 2, StudentId = 5, IsAttended = false, HomeworkMark = 0 },

                new AttendanceDb { LectureId = 3, StudentId = 1, IsAttended = true, HomeworkMark = 4 },
                new AttendanceDb { LectureId = 3, StudentId = 2, IsAttended = true, HomeworkMark = 3 },
                new AttendanceDb { LectureId = 3, StudentId = 3, IsAttended = true, HomeworkMark = 5 },
                new AttendanceDb { LectureId = 3, StudentId = 4, IsAttended = false, HomeworkMark = 0 },
                new AttendanceDb { LectureId = 3, StudentId = 5, IsAttended = false, HomeworkMark = 0 },

                new AttendanceDb { LectureId = 4, StudentId = 1, IsAttended = true, HomeworkMark = 4 },
                new AttendanceDb { LectureId = 4, StudentId = 2, IsAttended = true, HomeworkMark = 3 },
                new AttendanceDb { LectureId = 4, StudentId = 3, IsAttended = true, HomeworkMark = 5 },
                new AttendanceDb { LectureId = 4, StudentId = 4, IsAttended = false, HomeworkMark = 0 },
                new AttendanceDb { LectureId = 4, StudentId = 5, IsAttended = false, HomeworkMark = 0 },
            };

            context.Attendances.AddRange(attendances);
            context.SaveChanges();

            // Calculating student average grades for dataset
            var studentAvg = attendances.GroupBy(i => i.StudentId).Select(j => (j.Key, j.Average(k => k.HomeworkMark)));
            foreach (var student in students)
            {
                if (studentAvg.Select(i => i.Key).Contains(student.Id))
                {
                    student.StudentAverageGrade = studentAvg.First(i => i.Key == student.Id).Item2;
                }
            }

            context.Students.UpdateRange(students);
            context.SaveChanges();

            _logger.LogInformation("Database has been seeded.");
        }
    }
}