using AutoMapper;
using UniversityDomain.DomainEntites;

namespace M10_RestApi.ModelsDto
{
    public class RestApiMapperProfile : Profile
    {
        public RestApiMapperProfile()
        {
            CreateMap<StudentDto, Student>().ReverseMap();
            CreateMap<StudentPostDto, Student>().ReverseMap();
            CreateMap<TeacherDto, Teacher>().ReverseMap();
            CreateMap<TeacherPostDto, Teacher>().ReverseMap();
            CreateMap<Lecture, LectureDto>().ReverseMap();
            CreateMap<Lecture, LecturePostDto>().ReverseMap();
            CreateMap<AttendanceDto, Attendance>().ReverseMap();
            CreateMap<AttendancePostDto, Attendance>().ReverseMap();
            CreateMap<AverageGradeDto, AverageGrade>().ReverseMap();
        }
    }
}