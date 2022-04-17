using System;
using BusinessLogic.DomainEntityValidation;
using BusinessLogic.EntityServices;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class TeacherServiceTests
    {
        private IEntityRepository<ITeacher> _repositoryService;
        private IEntityValidation _validation;

        [OneTimeSetUp]
        public void Setup()
        {
            _repositoryService = new Mock<IEntityRepository<ITeacher>>().Object;
            _validation = new Mock<IEntityValidation>().Object;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _repositoryService = null;
            _validation = null;
        }

        [Test]
        public void CreateAndEditEntity_GivenNullAttendence_ThrowArgumentNullException()
        {
            // Arrange
            Teacher teacher = null;
            TeacherService service = new(_repositoryService, _validation);

            // Act
            Action createWithNull = () => service.CreateEntity(teacher);
            Action editWithNull = () => service.EditEntity(teacher);

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