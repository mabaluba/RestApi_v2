using System;
using System.ComponentModel.DataAnnotations;
using UniversityDomain.EntityInterfaces;

namespace UniversityDomain.DomainEntites
{
    public class Lecture : ILecture
    {
        public int Id { get; set; }

        [Required]
        public string Topic { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int TeacherId { get; set; }
    }
}