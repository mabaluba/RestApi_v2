using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;

namespace M10_RestApi.Controllers
{
    [ApiController]
    [Route("/api/education/teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly IEntityService<ITeacher> _entityService;
        private readonly IMapper _mapper;

        public TeacherController(IMapper mapper, IEntityService<ITeacher> entityService)
        {
            _mapper = mapper;
            _entityService = entityService;
        }

        [HttpGet("{id}")]
        public ActionResult<TeacherDto> GetTeacher(int id)
        {
            var teacher = _entityService.GetEntity(id);
            return teacher == null ? NotFound($"Teacher with Id = '{id}' not found.") : Ok(_mapper.Map<TeacherDto>(teacher));
        }

        [HttpGet("teachers")]
        public ActionResult<IReadOnlyCollection<TeacherDto>> GetTeachers()
        {
            var result = _entityService.GetAllEntities().Select(i => _mapper.Map<TeacherDto>(i)).ToArray();
            return result.Length == 0 ? NotFound($"Teachers not found.") : result;
        }

        [HttpPost]
        public ActionResult CreateTeacher(TeacherPostDto newTeacher)
        {
            var teacher = _entityService.CreateEntity(_mapper.Map<Teacher>(newTeacher));
            return Ok($"/api/education/teacher/{teacher.Id}");
        }

        [HttpPut("{id}")]
        public ActionResult EditTeacher(int id, TeacherDto teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest($"Requested Id = {id}, but given Id = {teacher.Id}");
            }

            var newTeacher = _entityService.EditEntity(_mapper.Map<Teacher>(teacher));
            return Ok($"/api/education/teacher/{newTeacher.Id}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTeacher(int id)
        {
            _entityService.DeleteEntity(id);
            return Ok();
        }
    }
}