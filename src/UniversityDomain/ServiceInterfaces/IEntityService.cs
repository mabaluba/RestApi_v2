using System.Collections.Generic;
using EducationDomain.EntityInterfaces;

namespace EducationDomain.ServiceInterfaces
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