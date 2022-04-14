using System;
using System.ComponentModel.DataAnnotations;
using EducationDomain.EntityInterfaces;

namespace EducationDomain.DomainEntites
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