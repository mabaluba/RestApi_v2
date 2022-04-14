using System.ComponentModel.DataAnnotations;
using EducationDomain.EntityInterfaces;

namespace EducationDomain.DomainEntites
{
    public class Student : IStudent
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
        public string PhoneNumber { get; set; }
    }
}