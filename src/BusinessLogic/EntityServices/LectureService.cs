using System;
using System.Collections.Generic;
using BusinessLogic.DomainEntityValidation;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    public class LectureService : IEntityService<ILecture>
    {
        private readonly IEntityRepository<ILecture> _repository;
        private readonly IEntityValidation _validation;

        public LectureService(IEntityRepository<ILecture> repository, IEntityValidation validation)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public ILecture CreateEntity(ILecture entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return _repository.CreateEntity(entity);
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
    }
}