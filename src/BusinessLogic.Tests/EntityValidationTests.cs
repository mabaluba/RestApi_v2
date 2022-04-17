using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.DomainEntityValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class EntityValidationTests
    {
        private ILogger<EntityValidation> _logger;
        private static readonly string _name = "name";

        [OneTimeSetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<EntityValidation>>().Object;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _logger = null;
        }

        [Test]
        public void Validate_GivenNull_ThrowArgumentNullException()
        {
            // Arrange
            IEntity entity = null;
            EntityValidation validation = new(_logger);

            // Act
            Action entityWithNull = () => validation.Validate(entity);

            // Assert
            Assert.That(entityWithNull, Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void EntityValidation_GivenNullArgs_ThrowArgumentNullException()
        {
            // Act
            Action loggerNull = () => new EntityValidation(null);

            // Assert
            Assert.That(loggerNull, Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [TestCaseSource(nameof(_attendancesThrow))]
        public void Validate_GivenEntity_ThrowValidationException(Attendance attendance)
        {
            // Act
            EntityValidation validation = new(_logger);
            Action notValid = () => validation.Validate(attendance);

            // Assert
            Assert.That(notValid, Throws.Exception.TypeOf<ValidationException>());
        }

        [TestCaseSource(nameof(_attendancesNotThrow))]
        public void Validate_GivenEntity_NotThrowValidationException(Attendance attendance)
        {
            // Act
            EntityValidation validation = new(_logger);
            Action notValid = () => validation.Validate(attendance);

            // Assert
            Assert.That(notValid, Throws.Nothing);
        }

        // TODO Tests for other entities.
        private static readonly List<Attendance> _attendancesThrow = new()
        {
            new()
            {
                Id = 0,
                LectureTopic = string.Empty,
                StudentFirstName = _name,
                StudentLastName = _name,
                IsAttended = false,
                HomeworkMark = 0
            },
            new()
            {
                Id = 1,
                LectureTopic = _name,
                StudentFirstName = string.Empty,
                StudentLastName = _name,
                IsAttended = false,
                HomeworkMark = 0
            },
            new()
            {
                Id = 1,
                LectureTopic = _name,
                StudentFirstName = _name,
                StudentLastName = string.Empty,
                IsAttended = false,
                HomeworkMark = 0
            },
            new()
            {
                Id = 1,
                LectureTopic = _name,
                StudentFirstName = _name,
                StudentLastName = _name,
                HomeworkMark = -1
            },
            new()
            {
                Id = 1,
                LectureTopic = _name,
                StudentFirstName = _name,
                StudentLastName = _name,
                IsAttended = false,
                HomeworkMark = 6
            }
        };

        private static readonly List<Attendance> _attendancesNotThrow = new()
        {
            new()
            {
                Id = 0,
                LectureTopic = _name,
                StudentFirstName = _name,
                StudentLastName = _name,
                IsAttended = false,
                HomeworkMark = 0
            },
            new()
            {
                Id = 1,
                LectureTopic = _name,
                StudentFirstName = _name,
                StudentLastName = _name,
                IsAttended = true,
                HomeworkMark = 0
            },
            new()
            {
                Id = 1,
                LectureTopic = _name,
                StudentFirstName = _name,
                StudentLastName = _name,
                IsAttended = false,
                HomeworkMark = 0
            },
            new()
            {
                Id = 1,
                LectureTopic = _name,
                StudentFirstName = _name,
                StudentLastName = _name,
                HomeworkMark = 0
            },
            new()
            {
                Id = 1,
                LectureTopic = _name,
                StudentFirstName = _name,
                StudentLastName = _name,
                IsAttended = false,
                HomeworkMark = 5
            }
        };
    }
}