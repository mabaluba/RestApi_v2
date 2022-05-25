using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.NotivicationServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.CourseControlServices
{
    internal class ControlService : IControlService
    {
        private readonly int _attendanceLevel = 3;
        private readonly int _averageGradeLevel = 4;

        private readonly ILogger<ControlService> _logger;
        private readonly IEntityRepositoryAsync<IAttendance> _repositoryAttendanceAsync;
        private readonly IEntityRepositoryAsync<ITeacher> _repositoryTeacherAsync;
        private readonly IEntityRepositoryAsync<ILecture> _repositoryLectureAsync;
        private readonly IAverageGradeRepositoryAsync<IAverageGrade> _repositoryAGRasync;
        private readonly IOptionsMonitor<EducationMailContacts> _options;

        public ControlService(
            ILogger<ControlService> logger,
            IAverageGradeRepositoryAsync<IAverageGrade> repositoryAGRasync,
            IOptionsMonitor<EducationMailContacts> options,
            IEntityRepositoryAsync<IAttendance> repositoryAttendanceAsync,
            IEntityRepositoryAsync<ITeacher> repositoryTeacherAsync,
            IEntityRepositoryAsync<ILecture> repositoryLectureAsync)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repositoryAGRasync = repositoryAGRasync ?? throw new ArgumentNullException(nameof(repositoryAGRasync));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _repositoryAttendanceAsync = repositoryAttendanceAsync ?? throw new ArgumentNullException(nameof(repositoryAttendanceAsync));
            _repositoryTeacherAsync = repositoryTeacherAsync ?? throw new ArgumentNullException(nameof(repositoryTeacherAsync));
            _repositoryLectureAsync = repositoryLectureAsync ?? throw new ArgumentNullException(nameof(repositoryLectureAsync));
        }

        // TODO : separate to several interfaces
        public async Task ControlStudentAsync(IAttendance entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var studentWithNewGrade = await UpdateGradeAsync(entity);
            await ControlAttendanceAsync(entity, studentWithNewGrade);
            await ControlGradeAsync(studentWithNewGrade);
        }

        private async Task<IAverageGrade> UpdateGradeAsync(IAttendance entity)
        {
            IAttendance[] concreteStudentAttendances = await FindStudentAttendancesAsync(entity);

            var averageGrade = concreteStudentAttendances.Length == 0 ? 0 : concreteStudentAttendances.Average(i => i.HomeworkMark);

            var studentGradeToSave = new AverageGrade()
            {
                FirstName = entity.StudentFirstName,
                LastName = entity.StudentLastName,
                StudentAverageGrade = averageGrade
            };

            return await _repositoryAGRasync.EditEntityAsync(studentGradeToSave);
        }

        private async Task<IAttendance[]> FindStudentAttendancesAsync(IAttendance entity)
        {
            var studentAttandances = await _repositoryAttendanceAsync.GetAllEntitiesAsync();
            var concreteStudentAttendances = studentAttandances.Where(i => i.StudentFirstName == entity.StudentFirstName && i.StudentLastName == entity.StudentLastName).ToArray();
            return concreteStudentAttendances;
        }

        private async Task ControlAttendanceAsync(IAttendance entity, IAverageGrade studentInfo)
        {
            var attendancesCount = await CountAttendancesAsync(entity);

            if (attendancesCount > _attendanceLevel)
            {
                var teachers = await _repositoryLectureAsync.GetAllEntitiesAsync();
                var teacherId = teachers.FirstOrDefault(i => i.Topic == entity.LectureTopic).TeacherId;

                var teacher = await _repositoryTeacherAsync.GetEntityAsync(teacherId);
                try
                {
                    SendEmailService emailService = new(_options);
                    await emailService.SendEmailAsync(studentInfo, teacher, attendancesCount);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message, exception);
                }
            }
        }

        private async Task<int> CountAttendancesAsync(IAttendance entity)
        {
            var attendances = await _repositoryAttendanceAsync.GetAllEntitiesAsync();
            var attendancesCount = attendances
                .Where(i =>
                    i.StudentFirstName == entity.StudentFirstName &&
                    i.StudentLastName == entity.StudentLastName &&
                    entity.IsAttended == false)
                .Count();
            return attendancesCount;
        }

        private async Task ControlGradeAsync(IAverageGrade studentInfo)
        {
            if (studentInfo.StudentAverageGrade < _averageGradeLevel)
            {
                try
                {
                    SendSmsService sendSmsService = new();
                    await sendSmsService.SendSmsAsync(studentInfo.PhoneNumber);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message, exception);
                }
            }
        }
    }
}