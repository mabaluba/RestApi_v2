using System.Collections.Generic;
using System.Threading.Tasks;
using EducationDomain.EntityInterfaces;

namespace EducationDomain.ServiceInterfaces
{
    public interface IEntityRepositoryAsync<T>
        where T : IEntity
    {
        Task<T> CreateEntityAsync(T attendance);

        Task DeleteEntityAsync(int attendanceId);

        Task<T> GetEntityAsync(int attendanceId);

        Task<IReadOnlyCollection<T>> GetAllEntitiesAsync();

        Task<T> EditEntityAsync(T attendance);
    }
}