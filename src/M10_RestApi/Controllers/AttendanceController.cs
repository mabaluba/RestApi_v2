using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.EntityServices;
using EducationDomain.DomainEntites;
using EducationDomain.EntityInterfaces;

// using EducationDomain.EntityInterfaces;
// using EducationDomain.ServiceInterfaces;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;

namespace M10_RestApi.Controllers
{
    [ApiController]
    [Route("/api/education/attendance")]
    public class AttendanceController : ControllerBase
    {
        // private readonly IEntityService<IAttendance> _entityService;
        private readonly IEntityServiceAsync<IAttendance> _entityServiceAsync;
        private readonly IMapper _mapper;

        public AttendanceController(
            IMapper mapper,

            // IEntityService<IAttendance> entityService,
            IEntityServiceAsync<IAttendance> entityServiceAsync)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            // _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
            _entityServiceAsync = entityServiceAsync;
        }

        // [HttpGet("{id}")]
        // public ActionResult<AttendanceDto> GetAttendance(int id)
        // {
        //    var attendance = _entityService.GetEntity(id);
        //    return attendance == null
        //        ? NotFound($"Attendance with Id = '{id}' not found.")
        //        : Ok(_mapper.Map<AttendanceDto>(attendance));
        // }
        //
        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceDto>> GetAttendanceAsync(int id)
        {
            var attendance = await _entityServiceAsync.GetEntityAsync(id);
            return attendance == null
                ? NotFound($"Attendance with Id = '{id}' not found.")
                : Ok(_mapper.Map<AttendanceDto>(attendance));
        }

        [HttpGet("attendances")]
        public async Task<ActionResult<IReadOnlyCollection<AttendanceDto>>> GetAttendancesAsync()
        {
            var collection = await _entityServiceAsync.GetAllEntitiesAsync();

            var result = collection.Select(i => _mapper.Map<AttendanceDto>(i)).ToArray();

            return result.Length == 0
            ? NotFound($"Attendances not found.")
            : Ok(result);
        }

        // [HttpPost]
        // public ActionResult CreateAttendance(AttendancePostDto newAttendance)
        // {
        //    var attendance = _entityService.CreateEntity(_mapper.Map<Attendance>(newAttendance));
        //    return Created($"/api/education/attendance/{attendance.Id}", attendance);
        // }
        //
        [HttpPost]
        public async Task<ActionResult> CreateAttendanceAsync(AttendancePostDto newAttendance)
        {
            var attendance = await _entityServiceAsync.CreateEntityAsync(_mapper.Map<Attendance>(newAttendance));
            return Created($"/api/education/attendance/{attendance.Id}", attendance);
        }

        // [HttpPut("{id}")]
        // public ActionResult EditAttendance(int id, AttendanceDto attendance)
        // {
        //    if (id != attendance.Id)
        //    {
        //        return BadRequest($"Requested Id = {id}, but given Id = {attendance.Id}. Please, correct input data.");
        //    }
        //
        //    var newAttendance = _entityService.EditEntity(_mapper.Map<Attendance>(attendance));
        //    return Ok($"/api/education/attendance/{newAttendance.Id}");
        // }
        //
        [HttpPut("{id}")]
        public async Task<ActionResult> EditAttendanceAsync(int id, AttendanceDto attendance)
        {
            if (id != attendance.Id)
            {
                return BadRequest($"Requested Id = {id}, but given Id = {attendance.Id}. Please, correct input data.");
            }

            var newAttendance = await _entityServiceAsync.EditEntityAsync(_mapper.Map<Attendance>(attendance));
            return Ok($"/api/education/attendance/{newAttendance.Id}");
        }

        // [HttpDelete("{id}")]
        // public ActionResult DeleteAttendance(int id)
        // {
        //     _entityService.DeleteEntity(id);
        //     return NoContent();
        // }
        //
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAttendanceAsync(int id)
        {
            await _entityServiceAsync.DeleteEntityAsync(id);
            return NoContent();
        }
    }
}