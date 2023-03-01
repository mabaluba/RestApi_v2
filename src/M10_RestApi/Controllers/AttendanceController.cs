using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Route("/api/education/attendance")]
    public class AttendanceController : ControllerBase
    {
        private readonly IEntityServiceAsync<IAttendance> _entityServiceAsync;
        private readonly IMapper _mapper;

        public AttendanceController(
            IMapper mapper,
            IEntityServiceAsync<IAttendance> entityServiceAsync)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityServiceAsync = entityServiceAsync ?? throw new ArgumentNullException(nameof(entityServiceAsync));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceDto>> GetAttendanceAsync(
            [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")] int id)
        {
            var attendance = await _entityServiceAsync.GetEntityAsync(id);
            return attendance == null
                ? NotFound($"Attendance with Id = '{id}' not found.")
                : Ok(_mapper.Map<AttendanceDto>(attendance));
        }

        [HttpGet("attendances")]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetAttendancesAsync()
        {
            var collection = await _entityServiceAsync.GetAllEntitiesAsync();

            return collection.Count == 0
                ? NotFound($"Attendances not found.")
                : Ok(collection.Select(i => _mapper.Map<AttendanceDto>(i)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateAttendanceAsync([FromBody] AttendancePostDto newAttendance)
        {
            var attendance = await _entityServiceAsync.CreateEntityAsync(_mapper.Map<Attendance>(newAttendance));
            return Created($"/api/education/attendance/{attendance.Id}", attendance);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditAttendanceAsync(
            [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")] int id,
            [FromBody] AttendanceDto attendance)
        {
            if (id != attendance.Id)
            {
                return BadRequest($"Requested Id = {id}, but given Id = {attendance.Id}. Please, correct input data.");
            }

            var newAttendance = await _entityServiceAsync.EditEntityAsync(_mapper.Map<Attendance>(attendance));
            return Ok($"/api/education/attendance/{newAttendance.Id}");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAttendanceAsync(
            [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")] int id)
        {
            await _entityServiceAsync.DeleteEntityAsync(id);
            return NoContent();
        }
    }
}