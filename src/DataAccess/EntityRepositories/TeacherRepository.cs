using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class TeacherRepository : IEntityRepository<ITeacher>, IEntityRepositoryAsync<ITeacher>
    {
        private readonly ILogger<TeacherRepository> _logger;
        private readonly IMapper _mapper;
        private readonly EducationDbContext _context;

        public TeacherRepository(EducationDbContext context, IMapper mapper, ILogger<TeacherRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public ITeacher CreateEntity(ITeacher teacher)
        {
            _ = teacher ?? throw new ArgumentNullException(nameof(teacher));

            var teacherDb = new TeacherDb()
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber
            };

            EntityEntry<TeacherDb> result;
            try
            {
                result = _context.Teachers?.Add(teacherDb);
                _context.SaveChanges();
                _logger.LogInformation($"Saved member with id = {teacherDb.Id} to database.");
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            return _mapper.Map<Teacher>(result?.Entity);
        }

        public ITeacher EditEntity(ITeacher teacher)
        {
            var teacherDb = _context.Teachers?.Find(teacher.Id)
                ?? throw new MissingMemberException($"Cannot find member with Id = {teacher.Id}.");

            teacherDb.FirstName = teacher.FirstName;
            teacherDb.LastName = teacher.LastName;
            teacherDb.Email = teacher.Email;
            teacherDb.PhoneNumber = teacher.PhoneNumber;
            try
            {
                _context.Update(teacherDb);
                _context.SaveChanges();
                _logger.LogInformation($"Saved changes for member with id = {teacherDb.Id} to database.");
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            return _mapper.Map<Teacher>(teacherDb);
        }

        public void DeleteEntity(int teacherId)
        {
            var teacherDb = _context.Teachers?.Find(teacherId)
                ?? throw new MissingMemberException($"Cannot find member with Id = {teacherId}.");

            _context.Teachers.Remove(teacherDb);
            _context.SaveChanges();
            _logger.LogInformation($"Delete member with id = {teacherId} from database.");
        }

        public IReadOnlyCollection<ITeacher> GetAllEntities()
        {
            var teachers = _context.Teachers?.ToArray() ?? throw new ArgumentNullException();
            _ = teachers.Length == 0 ? throw new MissingMemberException($"Cannot find any {typeof(Teacher).Name} members.") : 0;
            _logger.LogInformation($"Get all members {typeof(Teacher).Name} from database.");
            return _mapper.Map<IReadOnlyCollection<Teacher>>(teachers);
        }

        public ITeacher GetEntity(int teacherId)
        {
            var teacherDb = _context.Teachers?.Find(teacherId) ?? throw new MissingMemberException($"Cannot find member with Id = {teacherId}.");

            _logger.LogInformation($"Get member with id = {teacherId} from database.");
            return _mapper.Map<Teacher>(teacherDb);
        }

        public async Task<ITeacher> GetEntityAsync(int teacherId)
        {
            var teacherDb = await _context.Teachers.FindAsync(teacherId) ?? throw new MissingMemberException($"Cannot find member with Id = {teacherId}.");

            _logger.LogInformation($"Get member with id = {teacherId} from database.");
            return _mapper.Map<Teacher>(teacherDb);
        }

        public async Task<IReadOnlyCollection<ITeacher>> GetAllEntitiesAsync()
        {
            var teachers = await _context.Teachers?.ToArrayAsync();
            teachers = teachers.Length == 0 ? throw new MissingMemberException($"Cannot find any {typeof(Teacher).Name} members.") : teachers;
            _logger.LogInformation($"Get all members {typeof(Teacher).Name} from database.");
            return _mapper.Map<IReadOnlyCollection<Teacher>>(teachers);
        }

        public Task<ITeacher> CreateEntityAsync(ITeacher attendance)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEntityAsync(int attendanceId)
        {
            throw new NotImplementedException();
        }

        public Task<ITeacher> EditEntityAsync(ITeacher attendance)
        {
            throw new NotImplementedException();
        }
    }
}