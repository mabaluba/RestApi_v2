using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace DataAccess.EntityRepositories
{
    internal class AttendanceRepository : IEntityRepositoryAsync<IAttendance>
    {
        private readonly ILogger<AttendanceRepository> _logger;
        private readonly IMapper _mapper;
        private readonly EducationDbContext _context;

        public AttendanceRepository(EducationDbContext context, IMapper mapper, ILogger<AttendanceRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task<IAttendance> CreateEntityAsync(IAttendance attendance)
        {
            _ = attendance ?? throw new ArgumentNullException(nameof(attendance));

            var (lecture, student) = await CheckValidAttandanceAsync(attendance);

            EntityEntry<AttendanceDb> result;
            try
            {
                var attendanceDb = new AttendanceDb
                {
                    LectureId = lecture.Id,
                    StudentId = student.Id,
                    IsAttended = attendance.IsAttended,
                    HomeworkMark = attendance.HomeworkMark,
                };

                result = await _context.Attendances.AddAsync(attendanceDb);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Saved member with id = {attendanceDb.Id} to database.");
            }
            catch (Exception exception) when (exception is NullReferenceException || exception is DbUpdateException)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            return _mapper.Map<Attendance>(result.Entity);
        }

        public async Task<IAttendance> EditEntityAsync(IAttendance attendance)
        {
            _ = attendance ?? throw new ArgumentNullException(nameof(attendance));
            var (lecture, student) = await CheckValidAttandanceAsync(attendance);

            var attendanceDb = await _context.Attendances
                ?.Include(i => i.Lecture)
                ?.Include(j => j.Student)
                .FirstOrDefaultAsync(i => i.Id == attendance.Id)
                ?? throw new MissingMemberException($"Cannot find member with Id = {attendance.Id}.");

            attendanceDb.LectureId = lecture.Id;
            attendanceDb.StudentId = student.Id;
            attendanceDb.IsAttended = attendance.IsAttended;
            attendanceDb.HomeworkMark = attendance.HomeworkMark;

            try
            {
                _context.Update(attendanceDb);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Saved changes for member with id = {attendanceDb.Id} to database.");
            }
            catch (Exception exception) when (exception is InvalidOperationException || exception is NullReferenceException || exception is DbUpdateException)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            return _mapper.Map<Attendance>(attendanceDb);
        }

        public async Task DeleteEntityAsync(int attendanceId)
        {
            var attendanceDb = await _context.Attendances.FindAsync(attendanceId)
                ?? throw new MissingMemberException($"Cannot find member with Id = {attendanceId}.");

            _context.Attendances.Remove(attendanceDb);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Delete member with id = {attendanceId} from database.");
        }

        public async Task<IReadOnlyCollection<IAttendance>> GetAllEntitiesAsync()
        {
            var attendances = await _context.Attendances?
                .Include(i => i.Lecture)
                .Include(j => j.Student)
                .ToArrayAsync()
                ?? throw new ArgumentNullException();

            _ = attendances.Length == 0 ? throw new MissingMemberException($"Cannot find any {typeof(Attendance).Name} members.") : 0;

            _logger.LogInformation($"Get all members {typeof(Attendance).Name} from database.");
            return _mapper.Map<IReadOnlyCollection<Attendance>>(attendances);
        }

        public async Task<IAttendance> GetEntityAsync(int attendanceId)
        {
            var attendanceDb = await _context.Attendances?
                .Include(i => i.Lecture)
                .Include(j => j.Student)
                .FirstOrDefaultAsync(a => a.Id == attendanceId)
                ?? throw new MissingMemberException($"Cannot find member with Id = {attendanceId}.");

            _logger.LogInformation($"Get member with id = {attendanceId} from database.");
            return _mapper.Map<Attendance>(attendanceDb);
        }

        private async Task<(LectureDb Lecture, StudentDb Student)> CheckValidAttandanceAsync(IAttendance attendance)
        {
            _logger.LogInformation("Checking if given object has valid values.");
            var lecture = await _context.Lectures?.FirstOrDefaultAsync(i => i.Topic == attendance.LectureTopic) ?? throw new NullReferenceException();
            var student = await _context.Students?.FirstOrDefaultAsync(i => i.FirstName == attendance.StudentFirstName && i.LastName == attendance.StudentLastName) ?? throw new NullReferenceException();
            return (lecture, student);
        }
    }
}