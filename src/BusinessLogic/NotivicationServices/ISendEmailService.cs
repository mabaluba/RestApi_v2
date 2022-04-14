using System.Threading.Tasks;
using EducationDomain.EntityInterfaces;

namespace BusinessLogic.NotivicationServices
{
    public interface ISendEmailService
    {
        Task SendEmailAsync(IAverageGrade student, ITeacher teacher, int attendanceCount);
    }
}