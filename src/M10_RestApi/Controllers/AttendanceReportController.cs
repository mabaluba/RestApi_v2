using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.ReportServices;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UniversityDomain.EntityInterfaces;
using System.ComponentModel.DataAnnotations;

namespace M10_RestApi.Controllers
{
    using System.Net.Mime;

    [ApiController]
    [Route("/api/education/attendancereport")]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [ProducesResponseType(typeof(AttendanceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class AttendanceReportController : ControllerBase
    {
        private readonly ILogger<AttendanceController> _logger;
        private readonly IAttandanceReportService<IAttendance> _reportService;
        private readonly IMapper _mapper;

        public AttendanceReportController(
            IAttandanceReportService<IAttendance> reportService,
            IMapper mapper,
            ILogger<AttendanceController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("byLecture/{lectureTopic}")]
        public async Task<ActionResult<IReadOnlyCollection<AttendanceDto>>> GetAttendencesByTopicAsync(
            [Required(AllowEmptyStrings = false)] string lectureTopic)
        {
            _logger.LogInformation($"Requested attendance report with lecture topic '{lectureTopic}'.");
            var atendences = await _reportService.GetAttendencesByLectureTopicAsync(lectureTopic);
            var result = atendences.Select(i => _mapper.Map<AttendanceDto>(i)).ToArray();
            return result.Length == 0 ? NotFound($"There is no attendances with lecture topic '{lectureTopic}'.") : Ok(result);
        }

        [HttpPost("byStudent")]
        public async Task<ActionResult<IReadOnlyCollection<AttendanceDto>>> GetAttendencesByStudentNameAsync([FromBody] StudentFirstLastNameDto student)
        {
            _logger.LogInformation($"Requested attendance report for student '{student.FirstName} {student.LastName}'.");
            var attendences = await _reportService.GetAttendencesByStudentFistLastNameAsync(student.FirstName, student.LastName);
            var result = attendences.Select(i => _mapper.Map<AttendanceDto>(i)).ToArray();
            return result.Length == 0 ? NotFound($"There is no attendances with student '{student.FirstName} {student.LastName}'.") : Ok(result);
        }
    }
}