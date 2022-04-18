using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DomainEntityValidation;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    public class LectureService : IEntityServiceAsync<ILecture>
    {
        private readonly IEntityRepositoryAsync<ILecture> _repositoryAsync;
        private readonly IEntityValidation _validation;

        public LectureService(IEntityRepositoryAsync<ILecture> repositoryAsync, IEntityValidation validation)
        {
            _repositoryAsync = repositoryAsync ?? throw new ArgumentNullException(nameof(repositoryAsync));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public async Task<ILecture> CreateEntityAsync(ILecture entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return await _repositoryAsync.CreateEntityAsync(entity);
        }

        public async Task<ILecture> EditEntityAsync(ILecture entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return await _repositoryAsync.EditEntityAsync(entity);
        }

        public Task<IReadOnlyCollection<ILecture>> GetAllEntitiesAsync() => _repositoryAsync.GetAllEntitiesAsync();

        public async Task<ILecture> GetEntityAsync(int entityId) => await _repositoryAsync.GetEntityAsync(entityId);

        public async Task DeleteEntityAsync(int entityId) => await _repositoryAsync.DeleteEntityAsync(entityId);
    }
}