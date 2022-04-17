using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.ServiceInterfaces
{
    public interface IAverageGradeServiceAsync<T>
        where T : IAverageGrade
    {
        Task<T> GetEntityAsync(int entityId);

        Task<IReadOnlyCollection<T>> GetAllEntitiesAsync();
    }
}