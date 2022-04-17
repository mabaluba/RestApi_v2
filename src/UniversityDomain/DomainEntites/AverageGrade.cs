using System.ComponentModel.DataAnnotations;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.DomainEntites
{
    public class AverageGrade : IAverageGrade
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

        [Range(0, 5.0)]
        public double StudentAverageGrade { get; set; }
    }
}