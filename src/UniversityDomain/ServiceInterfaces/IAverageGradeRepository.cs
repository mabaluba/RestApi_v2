using System.Collections.Generic;
using EducationDomain.EntityInterfaces;

namespace EducationDomain.ServiceInterfaces
{
    public interface IAverageGradeRepository<T>
        where T : IAverageGrade
    {
        T GetEntity(int entityId);

        IReadOnlyCollection<T> GetAllEntities();

        T EditEntity(T entity);
    }
}