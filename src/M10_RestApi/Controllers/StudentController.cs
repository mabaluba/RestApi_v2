using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace M10_RestApi.Controllers
{
    [ApiController]
    [Route("/api/education/student")]
    public class StudentController : ControllerBase
    {
        private readonly IEntityServiceAsync<IStudent> _entityService;
        private readonly IMapper _mapper;

        public StudentController(IMapper mapper, IEntityServiceAsync<IStudent> entityService)
        {
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            _entityService = entityService ?? throw new System.ArgumentNullException(nameof(entityService));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentAsync(int id)
        {
            var student = await _entityService.GetEntityAsync(id);
            return student == null ? NotFound($"Student with Id = '{id}' not found.") : Ok(_mapper.Map<StudentDto>(student));
        }

        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsAsync()
        {
            var students = await _entityService.GetAllEntitiesAsync();
            return students.Count == 0 ? NotFound($"Students not found.") : Ok(students.Select(i => _mapper.Map<StudentDto>(i)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudentAsync(StudentPostDto student)
        {
            var newStudent = await _entityService.CreateEntityAsync(_mapper.Map<Student>(student));
            return Ok($"/api/education/student/{newStudent.Id}");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditStudentAsync(int id, StudentDto student)
        {
            if (id != student.Id)
            {
                return BadRequest($"Requested Id = {id}, but given Id = {student.Id}");
            }

            await _entityService.EditEntityAsync(_mapper.Map<Student>(student));
            return Ok($"/api/education/student/{student.Id}");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudentAsync(int id)
        {
            await _entityService.DeleteEntityAsync(id);
            return NoContent();
        }
    }
}