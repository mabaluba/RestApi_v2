using System.Collections.Generic;
using System.Threading.Tasks;
using EducationDomain.EntityInterfaces;

namespace EducationDomain.ServiceInterfaces
{
    public interface IAverageGradeRepositoryAsync<T>
        where T : IAverageGrade
    {
        Task<T> GetEntityAsync(int entityId);

        Task<T> EditEntityAsync(T student);

        Task<IReadOnlyCollection<T>> GetAllEntitiesAsync();
    }
}