using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    internal class AverageGradeServiceAsync : IAverageGradeServiceAsync<IAverageGrade>
    {
        private readonly IAverageGradeRepositoryAsync<IAverageGrade> _repository;

        public AverageGradeServiceAsync(IAverageGradeRepositoryAsync<IAverageGrade> repository)
        {
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public async Task<IAverageGrade> GetEntityAsync(int entityId) => await _repository.GetEntityAsync(entityId);

        public async Task<IReadOnlyCollection<IAverageGrade>> GetAllEntitiesAsync() => await _repository.GetAllEntitiesAsync();
    }
}