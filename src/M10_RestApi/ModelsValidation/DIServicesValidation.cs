using FluentValidation;
using M10_RestApi.ModelsDto;
using Microsoft.Extensions.DependencyInjection;

namespace M10_RestApi.ModelsValidation
{
    public static class DIServicesValidation
    {
        public static IServiceCollection AddRestApiValidationServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IValidator<StudentDto>, StudentValidator>()
                .AddTransient<IValidator<StudentPostDto>, StudentPostValidator>()
                .AddTransient<IValidator<TeacherDto>, TeacherValidator>()
                .AddTransient<IValidator<TeacherPostDto>, TeacherPostValidator>()
                .AddTransient<IValidator<LectureDto>, LectureValidator>()
                .AddTransient<IValidator<LecturePostDto>, LecturePostValidator>()
                .AddTransient<IValidator<AttendanceDto>, AttendanceValidator>()
                .AddTransient<IValidator<AttendancePostDto>, AttendancePostValidator>()
                .AddTransient<IValidator<AverageGradeDto>, AverageGradeValidator>()
                ;
        }
    }
}