using System;
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
    public class TeacherServiceTests
    {
        private IEntityRepositoryAsync<ITeacher> _repositoryService;
        private IEntityValidation _validation;

        [OneTimeSetUp]
        public void Setup()
        {
            _repositoryService = new Mock<IEntityRepositoryAsync<ITeacher>>().Object;
            _validation = new Mock<IEntityValidation>().Object;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _repositoryService = null;
            _validation = null;
        }

        [Test]
        public void CreateAndEditEntityAsync_GivenNullAttendence_ThrowArgumentNullException()
        {
            // Arrange
            Teacher teacher = null;
            TeacherService service = new(_repositoryService, _validation);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateEntityAsync(teacher));
                Assert.ThrowsAsync<ArgumentNullException>(async () => await service.EditEntityAsync(teacher));
            });
        }

        [Test]
        public void AttendanceEntityAsync_GivenNullArgs_ThrowArgumentNullException()
        {
            // Act
            Action repositoryNull = () => new TeacherService(null, _validation);
            Action validationNull = () => new TeacherService(_repositoryService, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(repositoryNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(validationNull, Throws.Exception.TypeOf<ArgumentNullException>());
            });
        }
    }
}