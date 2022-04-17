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
    public class StudentServiceTests
    {
        private IEntityRepository<IStudent> _repositoryService;
        private IEntityValidation _validation;

        [OneTimeSetUp]
        public void Setup()
        {
            _repositoryService = new Mock<IEntityRepository<IStudent>>().Object;
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
            Student student = null;
            StudentService service = new(_repositoryService, _validation);

            // Act
            Action createWithNull = () => service.CreateEntity(student);
            Action editWithNull = () => service.EditEntity(student);

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
            Action repositoryNull = () => new StudentService(null, _validation);
            Action validationNull = () => new StudentService(_repositoryService, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(repositoryNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(validationNull, Throws.Exception.TypeOf<ArgumentNullException>());
            });
        }
    }
}