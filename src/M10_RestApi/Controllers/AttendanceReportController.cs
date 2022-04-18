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

namespace M10_RestApi.Controllers
{
    [ApiController]
    [Route("/api/education/attendancereport")]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(AttendanceDto), StatusCodes.Status200OK)]
    public class AttendanceReportController : ControllerBase
    {
        private readonly ILogger<AttendanceController> _logger;
        private readonly IAttandanceReportService<IAttendance> _reportService;
        private readonly IMapper _mapper;

        public AttendanceReportController(IAttandanceReportService<IAttendance> reportService, IMapper mapper, ILogger<AttendanceController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("byLecture/{lectureTopic}")]
        public async Task<ActionResult<IReadOnlyCollection<AttendanceDto>>> GetAttendencesByTopicAsync(string lectureTopic)
        {
            var ttendences = await _reportService.GetAttendencesByLectureTopicAsync(lectureTopic);
            var result = ttendences.Select(i => _mapper.Map<AttendanceDto>(i)).ToArray();
            _logger.LogInformation($"Requested attendance report with lecture topic '{lectureTopic}'.");
            return result.Length == 0 ? NotFound($"There is no attendances with lecture topic '{lectureTopic}'.") : Ok(result);
        }

        [HttpGet("byStudent/{firstName},{lastName}")]
        public async Task<ActionResult<IReadOnlyCollection<AttendanceDto>>> GetAttendencesByStudentNameAsync(string firstName, string lastName)
        {
            var attendences = await _reportService.GetAttendencesByStudentFistLastNameAsync(firstName, lastName);
            var result = attendences.Select(i => _mapper.Map<AttendanceDto>(i)).ToArray();
            _logger.LogInformation($"Requested attendance report for student '{firstName} {lastName}'.");
            return result.Length == 0 ? NotFound($"There is no attendances with student '{firstName} {lastName}'.") : result;
        }
    }
}