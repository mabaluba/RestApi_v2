using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.CourseControlServices;
using BusinessLogic.DomainEntityValidation;
using BusinessLogic.EntityServices;
using Moq;
using NUnit.Framework;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class AttendanceServiceTests
    {
        private Mock<IEntityRepositoryAsync<IAttendance>> _repositoryServiceAsync;
        private IControlService _controlService;
        private IEntityValidation _validation;
        private IAttendance[] _attendances;

        [OneTimeSetUp]
        public void Setup()
        {
            _repositoryServiceAsync = new Mock<IEntityRepositoryAsync<IAttendance>>();
            _controlService = new Mock<IControlService>().Object;
            _validation = new Mock<IEntityValidation>().Object;
            _attendances = new DataForTests().AttandancesForTests;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _controlService = null;
            _validation = null;
            _attendances = null;
        }

        [Test]
        public void CreateAndEditEntity_GivenNullAttendence_ThrowArgumentNullException()
        {
            // Arrange
            Attendance attendance = null;
            AttendanceService service = new(_controlService, _validation, _repositoryServiceAsync.Object);

            // Assert
            Assert.Multiple(() =>
           {
               Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateEntityAsync(attendance));
               Assert.ThrowsAsync<ArgumentNullException>(async () => await service.EditEntityAsync(attendance));
           });
        }

        [Test]
        public void AttendanceEntity_GivenNullArgs_ThrowArgumentNullException()
        {
            // Act
            Action controlNull = () => new AttendanceService(null, _validation, _repositoryServiceAsync.Object);
            Action validationNull = () => new AttendanceService(_controlService, null, _repositoryServiceAsync.Object);
            Action repositoryNull = () => new AttendanceService(_controlService, _validation, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(repositoryNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(controlNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(validationNull, Throws.Exception.TypeOf<ArgumentNullException>());
            });
        }

        [Test]
        public async Task GetAllEntities_ReturnAllAttandancesFromDataForTests()
        {
            // Arrange
            _repositoryServiceAsync.Setup(i => i.GetAllEntitiesAsync()).ReturnsAsync(_attendances);
            var service = new AttendanceService(_controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            var res = await service.GetAllEntitiesAsync();

            // Assert
            Assert.That(res, Is.InstanceOf<IReadOnlyCollection<IAttendance>>());
            Assert.That(res, Has.Exactly(3).Items);
            Assert.That(res.Select(i => i.Id), Is.Unique);
        }

        [Test]
        public async Task GetEntity_GivenId_ReturnAttandancesFromDataForTests()
        {
            // Arrange
            var entityId = 2;
            _repositoryServiceAsync.Setup(i => i.GetEntityAsync(entityId)).ReturnsAsync(_attendances.First(i => i.Id == 2));
            var service = new AttendanceService(_controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            var res = await service.GetEntityAsync(entityId);

            // Assert
            Assert.That(res, Is.InstanceOf<IAttendance>());
            Assert.That(res.Id, Is.EqualTo(2));
        }

        [Test]
        public async Task CreateEntity_GetEntity_ReturnSavedNewEntity()
        {
            // Arrange
            Attendance entity = new()
            {
                Id = 0,
                LectureTopic = "LectureTopic",
                StudentFirstName = "Tom",
                StudentLastName = "Jerry",
                IsAttended = false,
                HomeworkMark = 0
            };

            Attendance entitySaved = new()
            {
                Id = 4,
                LectureTopic = "LectureTopic",
                StudentFirstName = "Tom",
                StudentLastName = "Jerry",
                IsAttended = false,
                HomeworkMark = 0
            };

            _repositoryServiceAsync.Setup(i => i.CreateEntityAsync(entity)).ReturnsAsync(entitySaved);
            var service = new AttendanceService(_controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            var res = await service.CreateEntityAsync(entity);

            // Assert
            Assert.That(res, Is.InstanceOf<IAttendance>());
            Assert.That(res, Is.EqualTo(entitySaved));
        }

        [Test]
        public async Task EditEntity_GetEntity_ReturnSavedChangesEntity()
        {
            // Arrange
            Attendance entity = new()
            {
                Id = 0,
                LectureTopic = "LectureTopic",
                StudentFirstName = "Tom",
                StudentLastName = "Jerry",
                IsAttended = false,
                HomeworkMark = 0
            };

            Attendance entitySaved = new()
            {
                Id = 4,
                LectureTopic = "LectureTopic",
                StudentFirstName = "Tom",
                StudentLastName = "Jerry",
                IsAttended = true,
                HomeworkMark = 5
            };

            _repositoryServiceAsync.Setup(i => i.EditEntityAsync(entity)).ReturnsAsync(entitySaved);
            var service = new AttendanceService(_controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            var res = await service.EditEntityAsync(entity);

            // Assert
            Assert.That(res, Is.InstanceOf<IAttendance>());
            Assert.That(res, Is.EqualTo(entitySaved));
        }

        [Test]
        public void DeleteEntity_GetEntity_ReturnSavedChangesEntity()
        {
            // Arrange
            var entityId = It.IsAny<int>();
            _repositoryServiceAsync.Setup(i => i.DeleteEntityAsync(entityId));
            var service = new AttendanceService(_controlService, _validation, _repositoryServiceAsync.Object);

            // Assert
            Action deletionInvoked = () => _repositoryServiceAsync.Verify(i => i.DeleteEntityAsync(entityId), Times.Once);
            Assert.DoesNotThrowAsync(async () => await service.DeleteEntityAsync(entityId));
            Assert.That(deletionInvoked, Throws.Nothing);
        }

        [Test]
        public void DeleteEntityAsync_GetEntity_ReturnSavedChangesEntity()
        {
            // Arrange
            var entityId = It.IsAny<int>();
            _repositoryServiceAsync.Setup(i => i.DeleteEntityAsync(entityId));
            var service = new AttendanceService(_controlService, _validation, _repositoryServiceAsync.Object);

            // Assert
            Action deletionInvoked = () => _repositoryServiceAsync.Verify(i => i.DeleteEntityAsync(entityId), Times.Once);
            Assert.DoesNotThrowAsync(async () => await service.DeleteEntityAsync(entityId));
        }
    }
}