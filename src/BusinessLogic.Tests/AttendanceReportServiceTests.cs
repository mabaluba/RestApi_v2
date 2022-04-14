using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.EntityServices;
using BusinessLogic.ReportServices;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class AttendanceReportServiceTests
    {
        private readonly string _name = "name";
        private readonly string _nameLast = "nameLast";
        private Mock<IEntityRepository<IAttendance>> _repositoryService;
        private ILogger<AttendanceService> _logger;
        private IAttendance[] _attendances;

        [OneTimeSetUp]
        public void Setup()
        {
            _repositoryService = new Mock<IEntityRepository<IAttendance>>();
            _logger = new Mock<ILogger<AttendanceService>>().Object;
            _attendances = new DataForTests().AttandancesForTests;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _repositoryService = null;
            _logger = null;
            _attendances = null;
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void GetAttendencesByLectureTopic_GivenNullLectureTopic_ThrowArgumentException(string lectureTopic)
        {
            // Arrange
            AttendanceReportService service = new(_repositoryService.Object, _logger);

            // Act
            Action topicWithNull = () => service.GetAttendencesByLectureTopic(lectureTopic);

            // Assert
            Assert.That(topicWithNull, Throws.Exception.TypeOf<ArgumentException>());
        }

        [TestCase(null, "name")]
        [TestCase(" ", "name")]
        [TestCase("name", null)]
        [TestCase("name", " ")]
        public void GetAttendencesByStudentFistLastName_GivenNullNames_ThrowArgumentException(string firstName, string lastName)
        {
            // Arrange
            AttendanceReportService service = new(_repositoryService.Object, _logger);

            // Act
            Action nameWithNull = () => service.GetAttendencesByStudentFistLastName(firstName, lastName);

            // Assert
            Assert.That(nameWithNull, Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void AttendanceEntity_GivenNullArgs_ThrowArgumentNullException()
        {
            // Act
            Action repositoryNull = () => new AttendanceReportService(null, _logger);
            Action loggerNull = () => new AttendanceReportService(_repositoryService.Object, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(repositoryNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(loggerNull, Throws.Exception.TypeOf<ArgumentNullException>());
            });
        }

        [Test]
        public void GetAttendencesByLectureTopic_GivenLectureTopic_ReturnAttandances()
        {
            // Arrange
            _repositoryService.Setup(i => i.GetAllEntities()).Returns(_attendances);
            var service = new AttendanceReportService(_repositoryService.Object, _logger);

            // Act
            var res = service.GetAttendencesByLectureTopic("LectureTopic");

            // Assert
            Assert.That(res, Is.InstanceOf<IReadOnlyCollection<IAttendance>>());
            Assert.That(res, Has.Exactly(3).Items);
            Assert.That(res.Select(i => i.LectureTopic), Is.All.EqualTo("LectureTopic"));
        }

        [Test]
        public void GetAttendencesByStudentFistLastName_GivenNames_ReturnAttandances()
        {
            // Arrange
            _repositoryService.Setup(i => i.GetAllEntities()).Returns(_attendances);
            var service = new AttendanceReportService(_repositoryService.Object, _logger);

            // Act
            var res = service.GetAttendencesByStudentFistLastName(_name, _nameLast);

            // Assert
            Assert.That(res, Is.InstanceOf<IReadOnlyCollection<IAttendance>>());
            Assert.That(res, Has.Exactly(3).Items);
            Assert.That(res.Select(i => (i.StudentFirstName, i.StudentLastName)), Is.All.EqualTo((_name, _nameLast)));
        }
    }
}