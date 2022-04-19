using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DomainEntityValidation;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    public class TeacherService : IEntityServiceAsync<ITeacher>
    {
        private readonly IEntityRepositoryAsync<ITeacher> _repository;
        private readonly IEntityValidation _validation;

        public TeacherService(IEntityRepositoryAsync<ITeacher> repository, IEntityValidation validation)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public async Task<ITeacher> CreateEntityAsync(ITeacher entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return await _repository.CreateEntityAsync(entity);
        }

        public async Task<ITeacher> EditEntityAsync(ITeacher entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return await _repository.EditEntityAsync(entity);
        }

        public async Task<IReadOnlyCollection<ITeacher>> GetAllEntitiesAsync() => await _repository.GetAllEntitiesAsync();

        public async Task<ITeacher> GetEntityAsync(int entityId) => await _repository.GetEntityAsync(entityId);

        public async Task DeleteEntityAsync(int entityId) => await _repository.DeleteEntityAsync(entityId);
    }
}