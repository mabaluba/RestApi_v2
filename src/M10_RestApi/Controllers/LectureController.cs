using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EducationDomain.DomainEntites;
using EducationDomain.EntityInterfaces;
using EducationDomain.ServiceInterfaces;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;

namespace M10_RestApi.Controllers
{
    [ApiController]
    [Route("/api/education/lecture")]
    public class LectureController : ControllerBase
    {
        private readonly IEntityService<ILecture> _entityService;
        private readonly IMapper _mapper;

        public LectureController(IMapper mapper, IEntityService<ILecture> entityService)
        {
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            _entityService = entityService ?? throw new System.ArgumentNullException(nameof(entityService));
        }

        [HttpGet("{id}")]
        public ActionResult<LectureDto> GetLecture(int id)
        {
            var lecture = _entityService.GetEntity(id);
            return lecture == null ? NotFound($"Lecture with Id = '{id}' not found.") : _mapper.Map<LectureDto>(lecture);
        }

        [HttpGet("lectures")]
        public ActionResult<IReadOnlyCollection<LectureDto>> GetLectures()
        {
            var result = _entityService.GetAllEntities().Select(i => _mapper.Map<LectureDto>(i)).ToArray();
            return result.Length == 0 ? NotFound($"Lectures not found.") : result;
        }

        [HttpPost]
        public ActionResult CreateLecture(LecturePostDto newLecture)
        {
            var lecture = _entityService.CreateEntity(_mapper.Map<Lecture>(newLecture));
            return Ok($"/api/education/lecture/{lecture.Id}");
        }

        [HttpPut("{id}")]
        public ActionResult EditLecture(int id, LectureDto lecture)
        {
            if (id != lecture.Id)
            {
                return BadRequest($"Requested Id = {id}, but given Id = {lecture.Id}");
            }

            var newLecture = _entityService.EditEntity(_mapper.Map<Lecture>(lecture));
            return Ok($"/api/education/lecture/{newLecture.Id}");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteLecture(int id)
        {
            _entityService.DeleteEntity(id);
            return Ok();
        }
    }
}