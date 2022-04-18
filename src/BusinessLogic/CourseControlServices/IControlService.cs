using System.Threading.Tasks;
using UniversityDomain.EntityInterfaces;

namespace BusinessLogic.CourseControlServices
{
    public interface IControlService
    {
        Task ControlStudentAsync(IAttendance entity);
    }
}