using System.ComponentModel.DataAnnotations;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.DomainEntites
{
    public class Attendance : IAttendance
    {
        public int Id { get; set; }

        [Required]
        public string LectureTopic { get; set; }

        [Required]
        public string StudentFirstName { get; set; }

        [Required]
        public string StudentLastName { get; set; }

        [Required]
        public bool IsAttended { get; set; }

        [Required]
        [Range(0, 5)]
        public int HomeworkMark { get; set; }
    }
}