using System.Collections.Generic;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.ServiceInterfaces
{
    public interface IAverageGradeRepository<T>
        where T : IAverageGrade
    {
        T GetEntity(int entityId);

        IReadOnlyCollection<T> GetAllEntities();

        T EditEntity(T entity);
    }
}