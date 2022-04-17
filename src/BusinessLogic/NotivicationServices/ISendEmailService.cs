using System.Threading.Tasks;
using UniversityDomain.EntityInterfaces;

namespace BusinessLogic.NotivicationServices
{
    public interface ISendEmailService
    {
        Task SendEmailAsync(IAverageGrade student, ITeacher teacher, int attendanceCount);
    }
}