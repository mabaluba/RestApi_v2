﻿using System.Threading.Tasks;
using UniversityDomain.EntityInterfaces;

namespace BusinessLogic.CourseControlServices
{
    public interface IControlService
    {
        void ControlStudent(IAttendance entity);

        Task ControlStudentAsync(IAttendance entity);
    }
}