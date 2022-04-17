namespace UniversityDomain.EntityInterfaces
{
    public interface IPerson : IEntity
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}