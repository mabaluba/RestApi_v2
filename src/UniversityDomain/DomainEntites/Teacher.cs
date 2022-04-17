using System.ComponentModel.DataAnnotations;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.DomainEntites
{
    public class Teacher : ITeacher
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
        public string PhoneNumber { get; set; }
    }
}