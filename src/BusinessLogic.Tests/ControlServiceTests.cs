using System;
using BusinessLogic.CourseControlServices;
using BusinessLogic.NotivicationServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class ControlServiceTests
    {
        private ILogger<ControlService> _logger;
        private IEntityRepository<ILecture> _repositoryLecture;
        private IEntityRepository<ITeacher> _repositoryTeacher;
        private IAverageGradeRepository<IAverageGrade> _repositoryAGR;
        private IAverageGradeRepositoryAsync<IAverageGrade> _repositoryAGRasync;
        private IOptions<EducationMailContacts> _options;
        private IEntityRepositoryAsync<IAttendance> _repositoryAttendanceAsync;
        private IEntityRepositoryAsync<ITeacher> _repositoryTeacherAsync;
        private IEntityRepositoryAsync<ILecture> _repositoryLectureAsync;

        [OneTimeSetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ControlService>>().Object;
            _repositoryLecture = new Mock<IEntityRepository<ILecture>>().Object;
            _repositoryTeacher = new Mock<IEntityRepository<ITeacher>>().Object;
            _repositoryAGR = new Mock<IAverageGradeRepository<IAverageGrade>>().Object;
            _repositoryAGRasync = new Mock<IAverageGradeRepositoryAsync<IAverageGrade>>().Object;
            _options = new Mock<IOptions<EducationMailContacts>>().Object;
            _repositoryAttendanceAsync = new Mock<IEntityRepositoryAsync<IAttendance>>().Object;
            _repositoryTeacherAsync = new Mock<IEntityRepositoryAsync<ITeacher>>().Object;
            _repositoryLectureAsync = new Mock<IEntityRepositoryAsync<ILecture>>().Object;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _logger = null;
            _repositoryLecture = null;
            _repositoryTeacher = null;
            _repositoryAGR = null;
            _repositoryAGRasync = null;
            _options = null;
            _repositoryAttendanceAsync = null;
            _repositoryTeacherAsync = null;
            _repositoryLectureAsync = null;
        }

        [Test]
        public void ControlStudent_GivenNullAttendence_ThrowArgumentNullException()
        {
            // Arrange
            Attendance attendance = null;
            ControlService service = new(
                _logger,
                _repositoryLecture,
                _repositoryTeacher,
                _repositoryAGRasync,
                _options,
                _repositoryAttendanceAsync,
                _repositoryTeacherAsync,
                _repositoryLectureAsync,
                _repositoryAGR);

            // Act
            Action attandanceWithNull = async () => await service.ControlStudentAsync(attendance);

            // Assert
            Assert.That(attandanceWithNull, Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ControlStudent_GivenNullArgs_ThrowArgumentNullException()
        {
            // Act
            Action loggerNull = () => new ControlService(null, _repositoryLecture, _repositoryTeacher, _repositoryAGRasync, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync, _repositoryAGR);
            Action repositoryAttendanceNull = () => new ControlService(_logger, _repositoryLecture, _repositoryTeacher, _repositoryAGRasync, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync, _repositoryAGR);
            Action repositoryLectureNull = () => new ControlService(_logger, null, _repositoryTeacher, _repositoryAGRasync, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync, _repositoryAGR);
            Action repositoryTeacherNull = () => new ControlService(_logger, _repositoryLecture, null, _repositoryAGRasync, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync, _repositoryAGR);
            Action repositoryAGRNull = () => new ControlService(_logger, _repositoryLecture, _repositoryTeacher, null, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync, _repositoryAGR);
            Action optionsNull = () => new ControlService(_logger, _repositoryLecture, _repositoryTeacher, _repositoryAGRasync, null, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync, _repositoryAGR);
            Action repositoryAttendanceAsyncNull = () => new ControlService(_logger, _repositoryLecture, _repositoryTeacher, _repositoryAGRasync, _options, null, _repositoryTeacherAsync, _repositoryLectureAsync, _repositoryAGR);
            Action repositoryTeacherAsyncNull = () => new ControlService(_logger, _repositoryLecture, _repositoryTeacher, _repositoryAGRasync, _options, _repositoryAttendanceAsync, null, _repositoryLectureAsync, _repositoryAGR);
            Action repositoryLectureAsyncNull = () => new ControlService(_logger, _repositoryLecture, _repositoryTeacher, _repositoryAGRasync, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, null, _repositoryAGR);
            Action repositoryOtherNull = () => new ControlService(_logger, _repositoryLecture, _repositoryTeacher, _repositoryAGRasync, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(loggerNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(repositoryAttendanceNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(repositoryLectureNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(repositoryTeacherNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(repositoryAGRNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(optionsNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(repositoryAttendanceAsyncNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(repositoryTeacherAsyncNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(repositoryLectureAsyncNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(repositoryOtherNull, Throws.Exception.TypeOf<ArgumentNullException>());
            });
        }
    }
}