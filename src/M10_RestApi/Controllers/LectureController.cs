using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.EntityServices;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;

namespace M10_RestApi.Controllers
{
    [ApiController]
    [Route("/api/education/lecture")]
    public class LectureController : ControllerBase
    {
        private readonly IEntityServiceAsync<ILecture> _entityServiceAsync;
        private readonly IMapper _mapper;

        public LectureController(IMapper mapper, IEntityServiceAsync<ILecture> entityServiceAsync)
        {
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            _entityServiceAsync = entityServiceAsync ?? throw new System.ArgumentNullException(nameof(entityServiceAsync));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LectureDto>> GetLectureAsync(int id)
        {
            var lecture = await _entityServiceAsync.GetEntityAsync(id);
            return lecture == null ? NotFound($"Lecture with Id = '{id}' not found.") : Ok(_mapper.Map<LectureDto>(lecture));
        }

        [HttpGet("lectures")]
        public async Task<ActionResult<IEnumerable<LectureDto>>> GetLecturesAsync()
        {
            var lectures = await _entityServiceAsync.GetAllEntitiesAsync();

            return lectures.Count == 0
                ? NotFound($"Lectures not found.")
                : Ok(lectures.Select(i => _mapper.Map<LectureDto>(i)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateLectureAsync(LecturePostDto newLecture)
        {
            var lecture = await _entityServiceAsync.CreateEntityAsync(_mapper.Map<Lecture>(newLecture));
            return Created($"/api/education/lecture/{lecture.Id}", lecture);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditLectureAsync(int id, LectureDto lecture)
        {
            if (id != lecture.Id)
            {
                return BadRequest($"Requested Id = {id}, but given Id = {lecture.Id}");
            }

            var newLecture = await _entityServiceAsync.EditEntityAsync(_mapper.Map<Lecture>(lecture));
            return Ok($"/api/education/lecture/{newLecture.Id}");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLectureAsync(int id)
        {
            await _entityServiceAsync.DeleteEntityAsync(id);
            return NoContent();
        }
    }
}