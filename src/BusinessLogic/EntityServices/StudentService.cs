using System;
using System.Collections.Generic;
using BusinessLogic.DomainEntityValidation;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    public class StudentService : IEntityService<IStudent>
    {
        private readonly IEntityRepository<IStudent> _repository;
        private readonly IEntityValidation _validation;

        public StudentService(IEntityRepository<IStudent> repository, IEntityValidation validation)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public IStudent CreateEntity(IStudent entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return _repository.CreateEntity(entity);
        }

        public IStudent EditEntity(IStudent entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return _repository.EditEntity(entity);
        }

        public IReadOnlyCollection<IStudent> GetAllEntities() => _repository.GetAllEntities();

        public IStudent GetEntity(int entityId) => _repository.GetEntity(entityId);

        public void DeleteEntity(int entityId) => _repository.DeleteEntity(entityId);
    }
}