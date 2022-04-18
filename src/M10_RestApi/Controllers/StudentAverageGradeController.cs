using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace M10_RestApi.Controllers
{
    [ApiController]
    [Route("/api/education/averagegrade")]
    public class StudentAverageGradeController : ControllerBase
    {
        private readonly IAverageGradeServiceAsync<IAverageGrade> _entityServiceAsync;
        private readonly IMapper _mapper;

        public StudentAverageGradeController(IMapper mapper, IAverageGradeServiceAsync<IAverageGrade> entityServiceAsync)
        {
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            _entityServiceAsync = entityServiceAsync ?? throw new System.ArgumentNullException(nameof(entityServiceAsync));
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<AverageGradeDto>> GetAverageGradeAsync(int studentId)
        {
            var studentAG = await _entityServiceAsync.GetEntityAsync(studentId);
            return studentAG == null ? NotFound($"Student with Id = '{studentId}' not found.") : Ok(_mapper.Map<AverageGradeDto>(studentAG));
        }

        [HttpGet("allstudents")]
        public async Task<ActionResult<IEnumerable<AverageGradeDto>>> GetAverageGradesAsync()
        {
            var studentsAG = await _entityServiceAsync.GetAllEntitiesAsync();

            return studentsAG.Count == 0 ? NotFound($"Students not found.") : Ok(studentsAG.Select(i => _mapper.Map<AverageGradeDto>(i)));
        }
    }
}