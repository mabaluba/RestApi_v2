using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.EntityServices;
using BusinessLogic.ReportServices;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class AttendanceReportServiceTests
    {
        private readonly string _name = "name";
        private readonly string _nameLast = "nameLast";
        private Mock<IEntityRepositoryAsync<IAttendance>> _repositoryService;
        private ILogger<AttendanceService> _logger;
        private IAttendance[] _attendances;

        [OneTimeSetUp]
        public void Setup()
        {
            _repositoryService = new Mock<IEntityRepositoryAsync<IAttendance>>();
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

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAttendencesByLectureTopicAsync(lectureTopic));
        }

        [TestCase(null, "name")]
        [TestCase(" ", "name")]
        [TestCase("name", null)]
        [TestCase("name", " ")]
        public void GetAttendencesByStudentFistLastName_GivenNullNames_ThrowArgumentException(string firstName, string lastName)
        {
            // Arrange
            AttendanceReportService service = new(_repositoryService.Object, _logger);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAttendencesByStudentFistLastNameAsync(firstName, lastName));
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
        public async Task GetAttendencesByLectureTopic_GivenLectureTopic_ReturnAttandances()
        {
            // Arrange
            _repositoryService.Setup(i => i.GetAllEntitiesAsync()).ReturnsAsync(_attendances);
            var service = new AttendanceReportService(_repositoryService.Object, _logger);

            // Act
            var res = await service.GetAttendencesByLectureTopicAsync("LectureTopic");

            // Assert
            Assert.That(res, Is.InstanceOf<IReadOnlyCollection<IAttendance>>());
            Assert.That(res, Has.Exactly(3).Items);
            Assert.That(res.Select(i => i.LectureTopic), Is.All.EqualTo("LectureTopic"));
        }

        [Test]
        public async Task GetAttendencesByStudentFistLastName_GivenNames_ReturnAttandances()
        {
            // Arrange
            _repositoryService.Setup(i => i.GetAllEntitiesAsync()).ReturnsAsync(_attendances);
            var service = new AttendanceReportService(_repositoryService.Object, _logger);

            // Act
            var res = await service.GetAttendencesByStudentFistLastNameAsync(_name, _nameLast);

            // Assert
            Assert.That(res, Is.InstanceOf<IReadOnlyCollection<IAttendance>>());
            Assert.That(res, Has.Exactly(3).Items);
            Assert.That(res.Select(i => (i.StudentFirstName, i.StudentLastName)), Is.All.EqualTo((_name, _nameLast)));
        }
    }
}