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
    public class LectureServiceTests
    {
        private IEntityRepositoryAsync<ILecture> _repositoryServiceAsync;
        private IEntityValidation _validation;

        [OneTimeSetUp]
        public void Setup()
        {
            _repositoryServiceAsync = new Mock<IEntityRepositoryAsync<ILecture>>().Object;
            _validation = new Mock<IEntityValidation>().Object;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _repositoryServiceAsync = null;
            _validation = null;
        }

        [Test]
        public void CreateAndEditEntity_GivenNull_ThrowArgumentNullException()
        {
            // Arrange
            Lecture lecture = null;
            LectureService service = new(_repositoryServiceAsync, _validation);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateEntityAsync(lecture));
                Assert.ThrowsAsync<ArgumentNullException>(async () => await service.EditEntityAsync(lecture));
            });
        }

        [Test]
        public void AttendanceEntity_GivenNullArgs_ThrowArgumentNullException()
        {
            // Act
            Action repositoryAsyncNull = () => new LectureService(null, _validation);
            Action validationNull = () => new LectureService(_repositoryServiceAsync, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(repositoryAsyncNull, Throws.Exception.TypeOf<ArgumentNullException>());
                Assert.That(validationNull, Throws.Exception.TypeOf<ArgumentNullException>());
            });
        }
    }
}