namespace UniversityDomain.EntityInterfaces
{
    public interface IAttendance : IEntity
    {
        string LectureTopic { get; set; }

        string StudentFirstName { get; set; }

        string StudentLastName { get; set; }

        bool IsAttended { get; set; }

        int HomeworkMark { get; set; }
    }
}