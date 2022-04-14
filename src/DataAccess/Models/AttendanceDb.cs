namespace DataAccess.Models
{
    internal class AttendanceDb
    {
        public int Id { get; set; }

        public int LectureId { get; set; }

        public LectureDb Lecture { get; set; }

        public int StudentId { get; set; }

        public StudentDb Student { get; set; }

        public bool IsAttended { get; set; }

        public int HomeworkMark { get; set; }
    }
}