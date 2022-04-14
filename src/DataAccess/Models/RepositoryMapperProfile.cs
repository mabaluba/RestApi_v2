using AutoMapper;
using EducationDomain.DomainEntites;

namespace DataAccess.Models
{
    internal class RepositoryMapperProfile : Profile
    {
        public RepositoryMapperProfile()
        {
            CreateMap<StudentDb, AverageGrade>().ReverseMap();
            CreateMap<StudentDb, Student>().ReverseMap();
            CreateMap<TeacherDb, Teacher>().ReverseMap();
            CreateMap<LectureDb, Lecture>().ReverseMap();
            CreateMap<AttendanceDb, Attendance>().ReverseMap();
        }
    }
}