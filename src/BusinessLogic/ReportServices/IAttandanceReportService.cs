﻿using System.Collections.Generic;
using EducationDomain.EntityInterfaces;

namespace BusinessLogic.ReportServices
{
    public interface IAttandanceReportService<T>
        where T : IEntity
    {
        public IReadOnlyCollection<T> GetAttendencesByLectureTopic(string lectureTopic);
        public IReadOnlyCollection<T> GetAttendencesByStudentFistLastName(string FirstName, string lastName);
    }
}