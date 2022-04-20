using BusinessLogic.CourseControlServices;
using BusinessLogic.DomainEntityValidation;
using BusinessLogic.EntityServices;
using BusinessLogic.ReportServices;
using Microsoft.Extensions.DependencyInjection;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace BusinessLogic
{
    public static class DIServices
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            return services
                .AddScoped<IEntityServiceAsync<IStudent>, StudentService>()
                .AddScoped<IEntityServiceAsync<ITeacher>, TeacherService>()
                .AddScoped<IEntityServiceAsync<IAttendance>, AttendanceService>()
                .AddScoped<IEntityServiceAsync<ILecture>, LectureService>()
                .AddScoped<IAverageGradeServiceAsync<IAverageGrade>, AverageGradeServiceAsync>()
                .AddScoped<IAttandanceReportService<IAttendance>, AttendanceReportService>()
                .AddScoped<IControlService, ControlService>()
                .AddScoped<IEntityValidation, EntityValidation>()
                ;
        }
    }
}