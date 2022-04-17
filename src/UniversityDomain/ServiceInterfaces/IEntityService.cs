using System.Collections.Generic;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.ServiceInterfaces
{
    public interface IEntityService<T>
        where T : IEntity
    {
        T CreateEntity(T entity);

        T GetEntity(int entityId);

        IReadOnlyCollection<T> GetAllEntities();

        T EditEntity(T entity);

        void DeleteEntity(int entityId);
    }
}