using System.Collections.Generic;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    internal class AverageGradeService : IAverageGradeService<IAverageGrade>
    {
        private readonly IAverageGradeRepository<IAverageGrade> _repository;

        public AverageGradeService(IAverageGradeRepository<IAverageGrade> repository)
        {
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public IAverageGrade GetEntity(int entityId) => _repository.GetEntity(entityId);

        public IReadOnlyCollection<IAverageGrade> GetAllEntities() => _repository.GetAllEntities();
    }
}