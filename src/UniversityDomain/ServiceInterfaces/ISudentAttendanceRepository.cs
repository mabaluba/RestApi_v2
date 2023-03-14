using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.ServiceInterfaces;

public interface ISudentAttendanceRepository : IEntityRepositoryAsync<IAttendance>
{
    Task<IReadOnlyCollection<IAttendance>> GetAttandanceByFirstLastName(string firstName, string lastName);
}