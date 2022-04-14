using BusinessLogic.CourseControlServices;
using BusinessLogic.DomainEntityValidation;
using BusinessLogic.EntityServices;
using BusinessLogic.ReportServices;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
    public static class DIServices
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            return services
                .AddScoped<IEntityService<IStudent>, StudentService>()
                .AddScoped<IEntityService<ITeacher>, TeacherService>()
                .AddScoped<IEntityService<ILecture>, LectureService>()
                .AddScoped<IEntityService<IAttendance>, AttendanceService>()
                .AddScoped<IAverageGradeService<IAverageGrade>, AverageGradeService>()
                .AddScoped<IAttandanceReportService<IAttendance>, AttendanceReportService>()
                .AddScoped<IAttendanceServiceAsync, AttendanceService>()
                .AddScoped<IControlService, ControlService>()
                .AddScoped<IEntityValidation, EntityValidation>()
                ;
        }
    }
}