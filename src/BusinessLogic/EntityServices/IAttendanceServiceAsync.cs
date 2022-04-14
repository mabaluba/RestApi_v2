using System.Collections.Generic;
using System.Threading.Tasks;
using EducationDomain.EntityInterfaces;

namespace BusinessLogic.EntityServices
{
    public interface IAttendanceServiceAsync
    {
        Task<IAttendance> CreateEntityAsync(IAttendance entity);

        Task<IAttendance> EditEntityAsync(IAttendance entity);

        Task DeleteEntityAsync(int entityId);

        Task<IAttendance> GetEntityAsync(int entityId);

        Task<IReadOnlyCollection<IAttendance>> GetAllEntitiesAsync();
    }
}