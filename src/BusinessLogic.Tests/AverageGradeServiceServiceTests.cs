using System;
using BusinessLogic.EntityServices;
using NUnit.Framework;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class AverageGradeServiceServiceTests
    {
        [Test]
        public void AverageGradeService_GivenNullRepository_ThrowArgumentNullException()
        {
            // Act
            Action repositoryNull = () => new AverageGradeServiceAsync(null);

            // Assert
            Assert.That(repositoryNull, Throws.Exception.TypeOf<ArgumentNullException>());
        }
    }
}