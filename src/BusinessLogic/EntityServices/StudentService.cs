using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DomainEntityValidation;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    public class StudentService : IEntityServiceAsync<IStudent>
    {
        private readonly IEntityRepositoryAsync<IStudent> _repository;
        private readonly IEntityValidation _validation;

        public StudentService(IEntityRepositoryAsync<IStudent> repository, IEntityValidation validation)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public async Task<IStudent> CreateEntityAsync(IStudent entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return await _repository.CreateEntityAsync(entity);
        }

        public async Task<IStudent> EditEntityAsync(IStudent entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return await _repository.EditEntityAsync(entity);
        }

        public async Task<IReadOnlyCollection<IStudent>> GetAllEntitiesAsync() => await _repository.GetAllEntitiesAsync();

        public async Task<IStudent> GetEntityAsync(int entityId) => await _repository.GetEntityAsync(entityId);

        public async Task DeleteEntityAsync(int entityId) => await _repository.DeleteEntityAsync(entityId);
    }
}