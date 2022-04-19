using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.ServiceInterfaces
{
    public interface IEntityServiceAsync<T>
        where T : IEntity
    {
        Task<T> CreateEntityAsync(T entity);

        Task<T> EditEntityAsync(T entity);

        Task DeleteEntityAsync(int entityId);

        Task<T> GetEntityAsync(int entityId);

        Task<IReadOnlyCollection<T>> GetAllEntitiesAsync();
    }
}