using UniversityDomain.EntityInterfaces;

namespace BusinessLogic.DomainEntityValidation
{
    public interface IEntityValidation
    {
        void Validate<T>(T entity)
            where T : IEntity;
    }
}