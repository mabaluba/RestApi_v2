using System.Collections.Generic;
using System.Linq;
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
        private readonly IEntityService<IStudent> _entityService;
        private readonly IMapper _mapper;

        public StudentController(IMapper mapper, IEntityService<IStudent> entityService)
        {
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            _entityService = entityService ?? throw new System.ArgumentNullException(nameof(entityService));
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDto> GetStudent(int id)
        {
            var student = _entityService.GetEntity(id);
            return student == null ? NotFound($"Student with Id = '{id}' not found.") : _mapper.Map<StudentDto>(student);
        }

        [HttpGet("students")]
        public ActionResult<IReadOnlyCollection<StudentDto>> GetStudents()
        {
            var students = _entityService.GetAllEntities().Select(i => _mapper.Map<StudentDto>(i)).ToArray();
            return students.Length == 0 ? NotFound($"Students not found.") : students;
        }

        [HttpPost]
        public ActionResult CreateStudent(StudentPostDto student)
        {
            var newStudent = _entityService.CreateEntity(_mapper.Map<Student>(student));
            return Ok($"/api/education/student/{newStudent.Id}");
        }

        [HttpPut("{id}")]
        public ActionResult EditStudent(int id, StudentDto student)
        {
            if (id != student.Id)
            {
                return BadRequest($"Requested Id = {id}, but given Id = {student.Id}");
            }

            _entityService.EditEntity(_mapper.Map<Student>(student));
            return Ok($"/api/education/student/{student.Id}");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(int id)
        {
            _entityService.DeleteEntity(id);
            return Ok();
        }
    }
}