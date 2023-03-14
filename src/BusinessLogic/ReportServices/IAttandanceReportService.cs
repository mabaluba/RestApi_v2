using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityDomain.EntityInterfaces;

namespace BusinessLogic.ReportServices;

public interface IAttandanceReportService<T>
    where T : IEntity
{
    public Task<IReadOnlyCollection<IAttendance>> GetAttendencesByLectureTopicAsync(string lectureTopic);

    public Task<IReadOnlyCollection<IAttendance>> GetAttendencesByStudentFistLastNameAsync(string firstName, string lastName);
}