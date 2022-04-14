using System;

namespace DataAccess.Models
{
    internal class LectureDb
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public DateTime Date { get; set; }

        public int TeacherId { get; set; }
    }
}