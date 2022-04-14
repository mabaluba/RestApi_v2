using System.Collections.Generic;
using EducationDomain.EntityInterfaces;

namespace EducationDomain.ServiceInterfaces
{
    public interface IAverageGradeService<T>
        where T : IAverageGrade
    {
        T GetEntity(int entityId);

        IReadOnlyCollection<T> GetAllEntities();
    }
}