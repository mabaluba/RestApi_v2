using System.Collections.Generic;
using System.Threading.Tasks;
using EducationDomain.EntityInterfaces;

namespace EducationDomain.ServiceInterfaces
{
    public interface IAverageGradeRepository<T>
        where T : IAverageGrade
    {
        T GetEntity(int entityId);

        Task<T> EditEntityAsync(T student);

        IReadOnlyCollection<T> GetAllEntities();

        T EditEntity(T entity);
    }
}