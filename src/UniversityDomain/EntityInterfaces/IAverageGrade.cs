namespace UniversityDomain.EntityInterfaces
{
    public interface IAverageGrade : IEntity, IPerson
    {
        double StudentAverageGrade { get; set; }
    }
}