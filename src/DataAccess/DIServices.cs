using DataAccess.Data;
using DataAccess.EntityRepositories;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace DataAccess
{
    public static class DIServices
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, string connectionString)
        {
            services
                .AddAutoMapper(typeof(RepositoryMapperProfile))

                // .AddDbContext<EducationDbContext>(
                //    options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped)
                //
                .AddDbContext<EducationDbContext>(
                    options => options.UseNpgsql(connectionString))

                .AddSingleton<IDbInitializer, DbInitializer>()
                .AddScoped<IEntityRepository<IStudent>, StudentRepository>()
                .AddScoped<IEntityRepository<ITeacher>, TeacherRepository>()
                .AddScoped<IEntityRepositoryAsync<ITeacher>, TeacherRepository>()
                .AddScoped<IEntityRepository<ILecture>, LectureRepository>()
                .AddScoped<IEntityRepositoryAsync<ILecture>, LectureRepository>()
                //.AddScoped<IEntityRepository<IAttendance>, AttendanceRepository>()
                .AddScoped<IEntityRepositoryAsync<IAttendance>, AttendanceRepository>()
                .AddScoped<IAverageGradeRepository<IAverageGrade>, AverageGradeRepository>()
                .AddScoped<IAverageGradeRepositoryAsync<IAverageGrade>, AverageGradeRepositoryAsync>()
                ;
            return services;
        }
    }
}