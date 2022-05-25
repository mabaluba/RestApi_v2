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
        private IAverageGradeRepositoryAsync<IAverageGrade> _repositoryAGRasync;
        private IOptionsMonitor<EducationMailContacts> _options;
        private IEntityRepositoryAsync<IAttendance> _repositoryAttendanceAsync;
        private IEntityRepositoryAsync<ITeacher> _repositoryTeacherAsync;
        private IEntityRepositoryAsync<ILecture> _repositoryLectureAsync;

        [OneTimeSetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ControlService>>().Object;
            _repositoryAGRasync = new Mock<IAverageGradeRepositoryAsync<IAverageGrade>>().Object;
            _options = new Mock<IOptionsMonitor<EducationMailContacts>>().Object;
            _repositoryAttendanceAsync = new Mock<IEntityRepositoryAsync<IAttendance>>().Object;
            _repositoryTeacherAsync = new Mock<IEntityRepositoryAsync<ITeacher>>().Object;
            _repositoryLectureAsync = new Mock<IEntityRepositoryAsync<ILecture>>().Object;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _logger = null;
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
                _repositoryAGRasync,
                _options,
                _repositoryAttendanceAsync,
                _repositoryTeacherAsync,
                _repositoryLectureAsync);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.ControlStudentAsync(attendance));
        }

        [Test]
        public void ControlStudent_GivenNullArgs_ThrowArgumentNullException()
        {
            // Act
            Action loggerNull = () => new ControlService(null, _repositoryAGRasync, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync);
            Action repositoryAGRNull = () => new ControlService(_logger, null, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync);
            Action optionsNull = () => new ControlService(_logger, _repositoryAGRasync, null, _repositoryAttendanceAsync, _repositoryTeacherAsync, _repositoryLectureAsync);
            Action repositoryAttendanceAsyncNull = () => new ControlService(_logger, _repositoryAGRasync, _options, null, _repositoryTeacherAsync, _repositoryLectureAsync);
            Action repositoryTeacherAsyncNull = () => new ControlService(_logger, _repositoryAGRasync, _options, _repositoryAttendanceAsync, null, _repositoryLectureAsync);
            Action repositoryLectureAsyncNull = () => new ControlService(_logger, _repositoryAGRasync, _options, _repositoryAttendanceAsync, _repositoryTeacherAsync, null);

            // Assert
            Assert.That(loggerNull, Throws.Exception.TypeOf<ArgumentNullException>());
            Assert.That(repositoryAGRNull, Throws.Exception.TypeOf<ArgumentNullException>());
            Assert.That(optionsNull, Throws.Exception.TypeOf<ArgumentNullException>());
            Assert.That(repositoryAttendanceAsyncNull, Throws.Exception.TypeOf<ArgumentNullException>());
            Assert.That(repositoryTeacherAsyncNull, Throws.Exception.TypeOf<ArgumentNullException>());
            Assert.That(repositoryLectureAsyncNull, Throws.Exception.TypeOf<ArgumentNullException>());
        }
    }
}