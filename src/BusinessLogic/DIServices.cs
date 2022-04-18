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
                .AddScoped<IEntityService<IStudent>, StudentService>()
                .AddScoped<IEntityService<ITeacher>, TeacherService>()
                .AddScoped<IEntityService<ILecture>, LectureService>()
                .AddScoped<IEntityServiceAsync<IAttendance>, AttendanceService>()
                .AddScoped<IEntityServiceAsync<ILecture>, LectureService>()
                .AddScoped<IAverageGradeService<IAverageGrade>, AverageGradeService>()
                .AddScoped<IAverageGradeServiceAsync<IAverageGrade>, AverageGradeServiceAsync>()
                .AddScoped<IAttandanceReportService<IAttendance>, AttendanceReportService>()
                .AddScoped<IControlService, ControlService>()
                .AddScoped<IEntityValidation, EntityValidation>()
                ;
        }
    }
}