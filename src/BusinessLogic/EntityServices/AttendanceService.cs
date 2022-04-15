using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.CourseControlServices;
using BusinessLogic.DomainEntityValidation;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    public class AttendanceService : IEntityService<IAttendance>, IEntityServiceAsync<IAttendance>
    {
        private readonly IEntityRepository<IAttendance> _repositoryAttendance;
        private readonly IEntityRepositoryAsync<IAttendance> _repositoryAttendanceAsync;
        private readonly IControlService _controlService;
        private readonly IEntityValidation _validation;
        public AttendanceService(
            IEntityRepository<IAttendance> repository,
            IControlService controlService,
            IEntityValidation validation,
            IEntityRepositoryAsync<IAttendance> repositoryAttendanceAsync)
        {
            _repositoryAttendance = repository ?? throw new ArgumentNullException(nameof(repository));
            _controlService = controlService ?? throw new ArgumentNullException(nameof(controlService));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
            _repositoryAttendanceAsync = repositoryAttendanceAsync ?? throw new ArgumentNullException(nameof(repositoryAttendanceAsync));
        }

        public IReadOnlyCollection<IAttendance> GetAllEntities() => _repositoryAttendance.GetAllEntities();

        public async Task<IReadOnlyCollection<IAttendance>> GetAllEntitiesAsync() => await _repositoryAttendanceAsync.GetAllEntitiesAsync();

        public IAttendance GetEntity(int entityId) => _repositoryAttendance.GetEntity(entityId);

        public async Task<IAttendance> GetEntityAsync(int entityId) => await _repositoryAttendanceAsync.GetEntityAsync(entityId);

        public IAttendance CreateEntity(IAttendance entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            var result = _repositoryAttendance.CreateEntity(entity);

            _controlService.ControlStudent(result);

            return result;
        }

        public async Task<IAttendance> CreateEntityAsync(IAttendance entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            var result = await _repositoryAttendanceAsync.CreateEntityAsync(entity);

            await _controlService.ControlStudentAsync(result);

            return result;
        }

        public IAttendance EditEntity(IAttendance entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            var result = _repositoryAttendance.EditEntity(entity);

            _controlService.ControlStudent(entity);

            return result;
        }

        public async Task<IAttendance> EditEntityAsync(IAttendance entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            var result = await _repositoryAttendanceAsync.EditEntityAsync(entity);

            await _controlService.ControlStudentAsync(entity);

            return result;
        }

        public void DeleteEntity(int entityId)
        {
            var entity = GetEntity(entityId);

            _repositoryAttendance.DeleteEntity(entityId);

            _controlService.ControlStudent(entity);
        }

        public async Task DeleteEntityAsync(int entityId)
        {
            var entity = await GetEntityAsync(entityId);

            await _repositoryAttendanceAsync.DeleteEntityAsync(entityId);

            await _controlService.ControlStudentAsync(entity);
        }
    }
}