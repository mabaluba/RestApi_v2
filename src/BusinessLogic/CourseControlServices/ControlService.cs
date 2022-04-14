using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.NotivicationServices;
using EducationDomain.DomainEntites;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessLogic.CourseControlServices
{
    internal class ControlService : IControlService
    {
        private readonly int _attendanceLevel = 3;
        private readonly int _averageGradeLevel = 4;

        private readonly ILogger<ControlService> _logger;
        private readonly IEntityRepository<IAttendance> _repositoryAttendance;
        private readonly IEntityRepositoryAsync<IAttendance> _repositoryAttendanceAsync;
        private readonly IEntityRepositoryAsync<ITeacher> _repositoryTeacherAsync;
        private readonly IEntityRepositoryAsync<ILecture> _repositoryLectureAsync;
        private readonly IEntityRepository<ILecture> _repositoryLecture;
        private readonly IEntityRepository<ITeacher> _repositoryTeacher;
        private readonly IAverageGradeRepository<IAverageGrade> _repositoryAGR;
        private readonly IOptions<EducationMailContacts> _options;

        public ControlService(
            ILogger<ControlService> logger,
            IEntityRepository<IAttendance> repositoryAttendance,
            IEntityRepository<ILecture> repositoryLecture,
            IEntityRepository<ITeacher> repositoryTeacher,
            IAverageGradeRepository<IAverageGrade> repositoryAGR,
            IOptions<EducationMailContacts> options,
            IEntityRepositoryAsync<IAttendance> repositoryAttendanceAsync,
            IEntityRepositoryAsync<ITeacher> repositoryTeacherAsync,
            IEntityRepositoryAsync<ILecture> repositoryLectureAsync)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repositoryAttendance = repositoryAttendance ?? throw new ArgumentNullException(nameof(repositoryAttendance));
            _repositoryLecture = repositoryLecture ?? throw new ArgumentNullException(nameof(repositoryLecture));
            _repositoryTeacher = repositoryTeacher ?? throw new ArgumentNullException(nameof(repositoryTeacher));
            _repositoryAGR = repositoryAGR ?? throw new ArgumentNullException(nameof(repositoryAGR));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _repositoryAttendanceAsync = repositoryAttendanceAsync ?? throw new ArgumentNullException(nameof(repositoryAttendanceAsync));
            _repositoryTeacherAsync = repositoryTeacherAsync ?? throw new ArgumentNullException(nameof(repositoryTeacherAsync));
            _repositoryLectureAsync = repositoryLectureAsync ?? throw new ArgumentNullException(nameof(repositoryLectureAsync));
        }

        public void ControlStudent(IAttendance entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var studentWithNewGrade = UpdateGrade(entity);
            ControlAttendance(entity, studentWithNewGrade);
            ControlGrade(studentWithNewGrade);
        }

        // TODO : separate to several interfaces
        public async Task ControlStudentAsync(IAttendance entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var studentWithNewGrade = await UpdateGradeAsync(entity);
            await ControlAttendanceAsync(entity, studentWithNewGrade);
            await ControlGradeAsync(studentWithNewGrade);
        }

        private IAverageGrade UpdateGrade(IAttendance entity)
        {
            var studentAttandances = _repositoryAttendance.GetAllEntities()
                .Where(i => i.StudentFirstName == entity.StudentFirstName && i.StudentLastName == entity.StudentLastName)
                .ToArray();
            var averageGrade = studentAttandances.Length == 0 ? 0 : studentAttandances.Average(i => i.HomeworkMark);

            var studentGradeToSave = new AverageGrade()
            {
                FirstName = entity.StudentFirstName,
                LastName = entity.StudentLastName,
                StudentAverageGrade = averageGrade
            };

            return _repositoryAGR.EditEntity(studentGradeToSave);
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

            return await _repositoryAGR.EditEntityAsync(studentGradeToSave);
        }

        private async Task<IAttendance[]> FindStudentAttendancesAsync(IAttendance entity)
        {
            var studentAttandances = await _repositoryAttendanceAsync.GetAllEntitiesAsync();
            var concreteStudentAttendances = studentAttandances.Where(i => i.StudentFirstName == entity.StudentFirstName && i.StudentLastName == entity.StudentLastName).ToArray();
            return concreteStudentAttendances;
        }

        private void ControlAttendance(IAttendance entity, IAverageGrade studentInfo)
        {
            var attendanceCount = _repositoryAttendance.GetAllEntities()
                .Where(i =>
                    i.StudentFirstName == entity.StudentFirstName &&
                    i.StudentLastName == entity.StudentLastName &&
                    entity.IsAttended == false)
                .Count()
                ;

            if (attendanceCount > _attendanceLevel)
            {
                var topic = entity.LectureTopic;
                var teacherId = _repositoryLecture.GetAllEntities().FirstOrDefault(i => i.Topic == topic).TeacherId;
                var teacher = _repositoryTeacher.GetEntity(teacherId);
                try
                {
                    SendEmailService emailService = new(_options);
                    emailService.SendEmailAsync(studentInfo, teacher, attendanceCount).RunSynchronously();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message, exception);
                }
            }
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

        private void ControlGrade(IAverageGrade studentInfo)
        {
            if (studentInfo.StudentAverageGrade < _averageGradeLevel)
            {
                try
                {
                    SendSmsService sendSmsService = new();
                    sendSmsService.SendSmsAsync(studentInfo.PhoneNumber).RunSynchronously();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message, exception);
                }
            }
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