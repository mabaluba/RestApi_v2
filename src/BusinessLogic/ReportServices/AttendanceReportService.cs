using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.ReportServices
{
    public class AttendanceReportService : IAttandanceReportService<IAttendance>
    {
        private readonly ILogger<AttendanceReportService> _logger;
        private readonly ISudentAttendanceRepository _attendanceRepository;

        public AttendanceReportService(ISudentAttendanceRepository attendanceRepository, ILogger<AttendanceReportService> logger)
        {
            _attendanceRepository = attendanceRepository ?? throw new ArgumentNullException(nameof(attendanceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IReadOnlyCollection<IAttendance>> GetAttendencesByLectureTopicAsync(string lectureTopic)
        {
            if (string.IsNullOrWhiteSpace(lectureTopic))
            {
                throw new ArgumentException($"'{nameof(lectureTopic)}' cannot be null or whitespace.", nameof(lectureTopic));
            }

            var lectureTopicLowerCase = lectureTopic.Trim().ToLower();
            var attandancesAll = await _attendanceRepository.GetAllEntitiesAsync();
            var attandances = attandancesAll.Where(i => i.LectureTopic.ToLower() == lectureTopicLowerCase).ToArray();
            _logger.LogInformation("Getting attendences by LectureTopic");
            return attandances;
        }

        public async Task<IReadOnlyCollection<IAttendance>> GetAttendencesByStudentFistLastNameAsync(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("First or Last name cannot be null or whitespace.");
            }

            var result = await _attendanceRepository.GetAttandanceByFirstLastName(firstName, lastName);
            _logger.LogInformation("Getting attendences by StudentFistLastName");
            return result;
        }
    }
}