using System;
using System.Collections.Generic;
using BusinessLogic.DomainEntityValidation;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    public class TeacherService : IEntityService<ITeacher>
    {
        private readonly IEntityRepository<ITeacher> _repository;
        private readonly IEntityValidation _validation;

        public TeacherService(IEntityRepository<ITeacher> repository, IEntityValidation validation)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public ITeacher CreateEntity(ITeacher entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return _repository.CreateEntity(entity);
        }

        public ITeacher EditEntity(ITeacher entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            return _repository.EditEntity(entity);
        }

        public IReadOnlyCollection<ITeacher> GetAllEntities() => _repository.GetAllEntities();

        public ITeacher GetEntity(int entityId) => _repository.GetEntity(entityId);

        public void DeleteEntity(int entityId) => _repository.DeleteEntity(entityId);
    }
}