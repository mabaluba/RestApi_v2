using System.Collections.Generic;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    internal class AverageGradeService : IAverageGradeService<IAverageGrade>
    {
        private readonly IAverageGradeRepository<IAverageGrade> _repository;

        public AverageGradeService(IAverageGradeRepository<IAverageGrade> repository)
        {
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public IReadOnlyCollection<IAverageGrade> GetAllEntities() => _repository.GetAllEntities();

        public IAverageGrade GetEntity(int entityId) => _repository.GetEntity(entityId);
    }
}