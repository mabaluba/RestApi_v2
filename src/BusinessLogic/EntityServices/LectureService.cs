using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DomainEntityValidation;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    public class LectureService : IEntityService<ILecture>, IEntityServiceAsync<ILecture>
    {
        private readonly IEntityRepository<ILecture> _repository;
        private readonly IEntityRepositoryAsync<ILecture> _repositoryAsync;
        private readonly IEntityValidation _validation;

        public LectureService(IEntityRepository<ILecture> repository, IEntityRepositoryAsync<ILecture> repositoryAsync, IEntityValidation validation)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _repositoryAsync = repositoryAsync ?? throw new ArgumentNullException(nameof(repositoryAsync));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public ILecture CreateEntity(ILecture entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return _repository.CreateEntity(entity);
        }

        public async Task<ILecture> CreateEntityAsync(ILecture entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return await _repositoryAsync.CreateEntityAsync(entity);
        }

        public ILecture EditEntity(ILecture entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return _repository.EditEntity(entity);
        }

        public IReadOnlyCollection<ILecture> GetAllEntities() => _repository.GetAllEntities();

        public ILecture GetEntity(int entityId) => _repository.GetEntity(entityId);

        public void DeleteEntity(int entityId) => _repository.DeleteEntity(entityId);

        public Task<ILecture> EditEntityAsync(ILecture entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEntityAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<ILecture> GetEntityAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<ILecture>> GetAllEntitiesAsync()
        {
            throw new NotImplementedException();
        }
    }
}