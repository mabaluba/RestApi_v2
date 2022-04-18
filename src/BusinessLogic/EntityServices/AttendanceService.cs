using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.CourseControlServices;
using BusinessLogic.DomainEntityValidation;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.EntityServices
{
    public class AttendanceService : IEntityServiceAsync<IAttendance>
    {
        private readonly IEntityRepositoryAsync<IAttendance> _repositoryAttendanceAsync;
        private readonly IControlService _controlService;
        private readonly IEntityValidation _validation;
        public AttendanceService(
            IControlService controlService,
            IEntityValidation validation,
            IEntityRepositoryAsync<IAttendance> repositoryAttendanceAsync)
        {
            _controlService = controlService ?? throw new ArgumentNullException(nameof(controlService));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
            _repositoryAttendanceAsync = repositoryAttendanceAsync ?? throw new ArgumentNullException(nameof(repositoryAttendanceAsync));
        }

        public async Task<IReadOnlyCollection<IAttendance>> GetAllEntitiesAsync() => await _repositoryAttendanceAsync.GetAllEntitiesAsync();

        public async Task<IAttendance> GetEntityAsync(int entityId) => await _repositoryAttendanceAsync.GetEntityAsync(entityId);

        public async Task<IAttendance> CreateEntityAsync(IAttendance entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _validation.Validate(entity);

            var result = await _repositoryAttendanceAsync.CreateEntityAsync(entity);

            await _controlService.ControlStudentAsync(result);

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

        public async Task DeleteEntityAsync(int entityId)
        {
            var entity = await GetEntityAsync(entityId);

            await _repositoryAttendanceAsync.DeleteEntityAsync(entityId);

            await _controlService.ControlStudentAsync(entity);
        }
    }
}