using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.CourseControlServices;
using BusinessLogic.DomainEntityValidation;
using BusinessLogic.EntityServices;
using EducationDomain.DomainEntites;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class AttendanceServiceTests
    {
        private Mock<IEntityRepository<IAttendance>> _repositoryService;
        private Mock<IEntityRepositoryAsync<IAttendance>> _repositoryServiceAsync;
        private IControlService _controlService;
        private IEntityValidation _validation;
        private IAttendance[] _attendances;

        [OneTimeSetUp]
        public void Setup()
        {
            _repositoryService = new Mock<IEntityRepository<IAttendance>>();
            _repositoryServiceAsync = new Mock<IEntityRepositoryAsync<IAttendance>>();
            _controlService = new Mock<IControlService>().Object;
            _validation = new Mock<IEntityValidation>().Object;
            _attendances = new DataForTests().AttandancesForTests;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _repositoryService = null;
            _controlService = null;
            _validation = null;
            _attendances = null;
        }

        [Test]
        public void CreateAndEditEntity_GivenNullAttendence_ThrowArgumentNullException()
        {
            // Arrange
            Attendance attendance = null;
            AttendanceService service = new(_repositoryService.Object, _controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            Action createWithNull = () => service.CreateEntity(attendance);
            Action editWithNull = () => service.EditEntity(attendance);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(createWithNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(editWithNull, Throws.Exception.TypeOf<ArgumentNullException>());
            });
        }

        [Test]
        public void AttendanceEntity_GivenNullArgs_ThrowArgumentNullException()
        {
            // Act
            Action repositoryNull = () => new AttendanceService(null, _controlService, _validation, _repositoryServiceAsync.Object);
            Action controlNull = () => new AttendanceService(_repositoryService.Object, null, _validation, _repositoryServiceAsync.Object);
            Action validationNull = () => new AttendanceService(_repositoryService.Object, _controlService, null, _repositoryServiceAsync.Object);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(repositoryNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(controlNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(validationNull, Throws.Exception.TypeOf<ArgumentNullException>());
            });
        }

        [Test]
        public void GetAllEntities_ReturnAllAttandancesFromDataForTests()
        {
            // Arrange
            _repositoryService.Setup(i => i.GetAllEntities()).Returns(_attendances);
            var service = new AttendanceService(_repositoryService.Object, _controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            var res = service.GetAllEntities();

            // Assert
            Assert.That(res, Is.InstanceOf<IReadOnlyCollection<IAttendance>>());
            Assert.That(res, Has.Exactly(3).Items);
            Assert.That(res.Select(i => i.Id), Is.Unique);
        }

        [Test]
        public void GetEntity_GivenId_ReturnAttandancesFromDataForTests()
        {
            // Arrange
            var entityId = 2;
            _repositoryService.Setup(i => i.GetEntity(entityId)).Returns(_attendances.First(i => i.Id == 2));
            var service = new AttendanceService(_repositoryService.Object, _controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            var res = service.GetEntity(entityId);

            // Assert
            Assert.That(res, Is.InstanceOf<IAttendance>());
            Assert.That(res.Id, Is.EqualTo(2));
        }

        [Test]
        public void CreateEntity_GetEntity_ReturnSavedNewEntity()
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

            _repositoryService.Setup(i => i.CreateEntity(entity)).Returns(entitySaved);
            var service = new AttendanceService(_repositoryService.Object, _controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            var res = service.CreateEntity(entity);

            // Assert
            Assert.That(res, Is.InstanceOf<IAttendance>());
            Assert.That(res, Is.EqualTo(entitySaved));
        }

        [Test]
        public void EditEntity_GetEntity_ReturnSavedChangesEntity()
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

            _repositoryService.Setup(i => i.EditEntity(entity)).Returns(entitySaved);
            var service = new AttendanceService(_repositoryService.Object, _controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            var res = service.EditEntity(entity);

            // Assert
            Assert.That(res, Is.InstanceOf<IAttendance>());
            Assert.That(res, Is.EqualTo(entitySaved));
        }

        [Test]
        public void DeleteEntity_GetEntity_ReturnSavedChangesEntity()
        {
            // Arrange
            var entityId = It.IsAny<int>();
            _repositoryService.Setup(i => i.DeleteEntity(entityId));
            var service = new AttendanceService(_repositoryService.Object, _controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            Action deletion = () => service.DeleteEntity(entityId);

            // Assert
            Action deletionInvoked = () => _repositoryService.Verify(i => i.DeleteEntity(entityId), Times.Once);
            Assert.That(deletion, Throws.Nothing);
            Assert.That(deletionInvoked, Throws.Nothing);
        }

        [Test]
        public void DeleteEntityAsync_GetEntity_ReturnSavedChangesEntity()
        {
            // Arrange
            var entityId = It.IsAny<int>();
            _repositoryServiceAsync.Setup(i => i.DeleteEntityAsync(entityId));
            var service = new AttendanceService(_repositoryService.Object, _controlService, _validation, _repositoryServiceAsync.Object);

            // Act
            Action deletion = () => service.DeleteEntityAsync(entityId).Wait();

            // Assert
            Action deletionInvoked = () => _repositoryServiceAsync.Verify(i => i.DeleteEntityAsync(entityId), Times.Once);
            Assert.That(deletion, Throws.Nothing);
            Assert.That(deletionInvoked, Throws.Nothing);
        }
    }
}