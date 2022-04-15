using System.Collections.Generic;
using System.Threading.Tasks;
using EducationDomain.EntityInterfaces;

namespace BusinessLogic.EntityServices
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