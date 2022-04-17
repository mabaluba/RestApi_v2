using System.Collections.Generic;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.ServiceInterfaces
{
    public interface IAverageGradeService<T>
        where T : IAverageGrade
    {
        T GetEntity(int entityId);

        IReadOnlyCollection<T> GetAllEntities();
    }
}