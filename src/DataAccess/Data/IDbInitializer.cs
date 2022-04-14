namespace DataAccess.Data
{
    public interface IDbInitializer
    {
        void Initialize(EducationDbContext context);
    }
}