using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.EntityServices;
using Microsoft.Extensions.Logging;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.ReportServices
{
    public class AttendanceReportService : IAttandanceReportService<IAttendance>
    {
        private readonly ILogger<AttendanceService> _logger;
        private readonly IEntityRepositoryAsync<IAttendance> _repository;

        public AttendanceReportService(IEntityRepositoryAsync<IAttendance> repository, ILogger<AttendanceService> logger)
        {
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task<IReadOnlyCollection<IAttendance>> GetAttendencesByLectureTopicAsync(string lectureTopic)
        {
            if (string.IsNullOrWhiteSpace(lectureTopic))
            {
                throw new System.ArgumentException($"'{nameof(lectureTopic)}' cannot be null or whitespace.", nameof(lectureTopic));
            }

            var lectureTopicLowerCase = lectureTopic.Trim().ToLower();
            var attandancesAll = await _repository.GetAllEntitiesAsync();
            var attandances = attandancesAll.Where(i => i.LectureTopic.ToLower() == lectureTopicLowerCase).ToArray();
            _logger.LogInformation("Getting attendences by LectureTopic");
            return attandances;
        }

        public async Task<IReadOnlyCollection<IAttendance>> GetAttendencesByStudentFistLastNameAsync(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new System.ArgumentException($"Arguments cannot be null or whitespace.");
            }

            var firstNameToLower = firstName.Trim().ToLower();
            var lastNameToLower = lastName.Trim().ToLower();
            var attandances = await _repository.GetAllEntitiesAsync();
            var result = attandances
                .Where(i =>
                    i.StudentFirstName.ToLower() == firstNameToLower &&
                    i.StudentLastName.ToLower() == lastNameToLower)
                .ToArray();
            _logger.LogInformation("Getting attendences by StudentFistLastName");
            return result;
        }
    }
}