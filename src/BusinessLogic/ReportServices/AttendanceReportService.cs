using System.Collections.Generic;
using System.Linq;
using BusinessLogic.EntityServices;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.ReportServices
{
    public class AttendanceReportService : IAttandanceReportService<IAttendance>
    {
        private readonly ILogger<AttendanceService> _logger;
        private readonly IEntityRepository<IAttendance> _repository;

        public AttendanceReportService(IEntityRepository<IAttendance> repository, ILogger<AttendanceService> logger)
        {
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public IReadOnlyCollection<IAttendance> GetAttendencesByLectureTopic(string lectureTopic)
        {
            if (string.IsNullOrWhiteSpace(lectureTopic))
            {
                throw new System.ArgumentException($"'{nameof(lectureTopic)}' cannot be null or whitespace.", nameof(lectureTopic));
            }

            var attandances = _repository
                .GetAllEntities()
                .Where(i => i.LectureTopic.ToLower() == lectureTopic.Trim().ToLower())
                .ToArray();
            _logger.LogInformation("Getting attendences by LectureTopic");
            return attandances;
        }

        public IReadOnlyCollection<IAttendance> GetAttendencesByStudentFistLastName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new System.ArgumentException($"Arguments cannot be null or whitespace.");
            }

            var attandances = _repository.GetAllEntities();
            var result = attandances
                .Where(i =>
                    i.StudentFirstName.ToLower() == firstName.Trim().ToLower() &&
                    i.StudentLastName.ToLower() == lastName.Trim().ToLower())
                .ToArray();
            _logger.LogInformation("Getting attendences by StudentFistLastName");
            return result;
        }
    }
}