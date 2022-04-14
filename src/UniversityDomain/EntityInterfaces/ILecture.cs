using System;

namespace EducationDomain.EntityInterfaces
{
    public interface ILecture : IEntity
    {
        public string Topic { get; set; }

        public DateTime Date { get; set; }

        public int TeacherId { get; set; }
    }
}