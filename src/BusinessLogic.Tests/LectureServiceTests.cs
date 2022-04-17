using System;

// using System.Threading.Tasks;
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
    public class LectureServiceTests
    {
        private IEntityRepository<ILecture> _repositoryService;
        private IEntityRepositoryAsync<ILecture> _repositoryServiceAsync;
        private IEntityValidation _validation;

        [OneTimeSetUp]
        public void Setup()
        {
            _repositoryService = new Mock<IEntityRepository<ILecture>>().Object;
            _repositoryServiceAsync = new Mock<IEntityRepositoryAsync<ILecture>>().Object;
            _validation = new Mock<IEntityValidation>().Object;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _repositoryService = null;
            _validation = null;
        }

        [Test]
        public void CreateAndEditEntity_GivenNull_ThrowArgumentNullException()
        {
            // Arrange
            Lecture lecture = null;
            LectureService service = new(_repositoryService, _repositoryServiceAsync, _validation);

            // Act
            Action createWithNull = () => service.CreateEntity(lecture);
            Action editWithNull = () => service.EditEntity(lecture);

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
            Action repositoryNull = () => new LectureService(null, _repositoryServiceAsync, _validation);
            Action repositoryAsyncNull = () => new LectureService(_repositoryService, null, _validation);
            Action validationNull = () => new LectureService(_repositoryService, _repositoryServiceAsync, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(repositoryNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(repositoryAsyncNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(validationNull, Throws.Exception.TypeOf<ArgumentNullException>());
            });
        }
    }
}