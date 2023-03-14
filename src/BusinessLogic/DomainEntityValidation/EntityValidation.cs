using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using UniversityDomain.EntityInterfaces;

namespace BusinessLogic.DomainEntityValidation
{
    internal class EntityValidation : IEntityValidation
    {
        private readonly ILogger<EntityValidation> _logger;

        public EntityValidation(ILogger<EntityValidation> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Validate<T>(T entity)
            where T : IEntity
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var vc = new ValidationContext(entity);
            var vr = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, vc, vr, true))
            {
                _logger.LogWarning($"Inner Validation model {typeof(T)} failed");
                throw new ValidationException($"Domain Model validation error in BusinessLogic for {typeof(T)}.");
            }
        }
    }
}