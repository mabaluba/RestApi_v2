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
    [Route("/api/education/teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly IEntityServiceAsync<ITeacher> _entityService;
        private readonly IMapper _mapper;

        public TeacherController(IMapper mapper, IEntityServiceAsync<ITeacher> entityService)
        {
            _mapper = mapper;
            _entityService = entityService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherDto>> GetTeacherAsync(int id)
        {
            var teacher = await _entityService.GetEntityAsync(id);
            return teacher == null ? NotFound($"Teacher with Id = '{id}' not found.") : Ok(_mapper.Map<TeacherDto>(teacher));
        }

        [HttpGet("teachers")]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachersAsync()
        {
            var teachers = await _entityService.GetAllEntitiesAsync();
            return teachers.Count == 0
                ? NotFound($"Teachers not found.")
                : Ok(teachers.Select(i => _mapper.Map<TeacherDto>(i)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateTeacherAsync(TeacherPostDto newTeacher)
        {
            var teacher = await _entityService.CreateEntityAsync(_mapper.Map<Teacher>(newTeacher));
            return Created($"/api/education/teacher/{teacher.Id}", teacher);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditTeacherAsync(int id, TeacherDto teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest($"Requested Id = {id}, but given Id = {teacher.Id}");
            }

            var newTeacher = await _entityService.EditEntityAsync(_mapper.Map<Teacher>(teacher));
            return Ok($"/api/education/teacher/{newTeacher.Id}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacherAsync(int id)
        {
            await _entityService.DeleteEntityAsync(id);
            return NoContent();
        }
    }
}