using UniversityDomain.DomainEntites;

namespace BusinessLogic.Tests
{
    internal static class DataForTests
    {
        private static readonly string _name = "name";
        private static readonly string _nameLast = "nameLast";

        internal static Attendance[] AttandancesForTests => new Attendance[]
        {
            new()
            {
                Id = 1,
                LectureTopic = "LectureTopic",
                StudentFirstName = _name,
                StudentLastName = _nameLast,
                IsAttended = false,
                HomeworkMark = 0
            },
            new()
            {
                Id = 2,
                LectureTopic = "LectureTopic",
                StudentFirstName = _name,
                StudentLastName = _nameLast,
                IsAttended = true,
                HomeworkMark = 3
            },
            new()
            {
                Id = 3,
                LectureTopic = "LectureTopic",
                StudentFirstName = _name,
                StudentLastName = _nameLast,
                IsAttended = true,
                HomeworkMark = 5
            }
        };
    }
}