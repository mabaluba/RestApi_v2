using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;

namespace M10_RestApi.Controllers
{
    [ApiController]
    [Route("/api/education/averagegrade")]
    public class StudentAverageGradeController : ControllerBase
    {
        private readonly IAverageGradeService<IAverageGrade> _entityService;
        private readonly IMapper _mapper;

        public StudentAverageGradeController(IMapper mapper, IAverageGradeService<IAverageGrade> entityService)
        {
            _mapper = mapper;
            _entityService = entityService;
        }

        [HttpGet("{studentId}")]
        public ActionResult<AverageGradeDto> GetAverageGrade(int studentId)
        {
            var studentAG = _entityService.GetEntity(studentId);
            return studentAG == null ? NotFound($"Student with Id = '{studentId}' not found.") : _mapper.Map<AverageGradeDto>(studentAG);
        }

        [HttpGet("allstudents")]
        public ActionResult<IReadOnlyCollection<AverageGradeDto>> GetAverageGrades()
        {
            var studentsAG = _entityService.GetAllEntities().Select(i => _mapper.Map<AverageGradeDto>(i)).ToArray();
            return studentsAG.Length == 0 ? NotFound($"Students not found.") : studentsAG;
        }
    }
}