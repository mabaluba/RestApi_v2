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
    internal class StudentRepository : IEntityRepositoryAsync<IStudent>
    {
        private readonly ILogger<StudentRepository> _logger;
        private readonly IMapper _mapper;
        private readonly EducationDbContext _context;

        public StudentRepository(EducationDbContext context, IMapper mapper, ILogger<StudentRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task<IStudent> CreateEntityAsync(IStudent student)
        {
            _ = student ?? throw new ArgumentNullException(nameof(student));

            var studentDb = new StudentDb()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber
            };

            EntityEntry<StudentDb> result;
            try
            {
                result = await _context.Students.AddAsync(studentDb);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Saved member with id = {studentDb.Id} to database.");
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, exception.Message, studentDb);
                throw;
            }

            return _mapper.Map<Student>(result.Entity);
        }

        public async Task<IStudent> EditEntityAsync(IStudent student)
        {
            var studentDb = await _context.Students.FindAsync(student.Id) ?? throw new MissingMemberException($"Cannot find member with Id = {student.Id}.");

            studentDb.FirstName = student.FirstName;
            studentDb.LastName = student.LastName;
            studentDb.Email = student.Email;
            studentDb.PhoneNumber = student.PhoneNumber;
            try
            {
                _context.Update(studentDb);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Saved changes for member with id = {studentDb.Id} to database.");
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            return _mapper.Map<Student>(studentDb);
        }

        public async Task DeleteEntityAsync(int studentId)
        {
            var studentDb = await _context.Students.FindAsync(studentId) ?? throw new MissingMemberException($"Cannot find member with Id = {studentId}.");

            _context.Students.Remove(studentDb);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Delete member with id = {studentId} from database.");
        }

        public async Task<IReadOnlyCollection<IStudent>> GetAllEntitiesAsync()
        {
            var students = await _context.Students?.ToArrayAsync();
            _ = students.Length == 0 ? throw new MissingMemberException($"Cannot find any {typeof(Student).Name} members.") : 0;

            _logger.LogInformation($"Get all members {typeof(Student).Name} from database.");
            return _mapper.Map<IReadOnlyCollection<Student>>(students);
        }

        public async Task<IStudent> GetEntityAsync(int studentId)
        {
            var studentDb = await _context.Students.FindAsync(studentId) ?? throw new MissingMemberException($"Cannot find member with Id = {studentId}.");

            _logger.LogInformation($"Get member with id = {studentId} from database.");
            return _mapper.Map<Student>(studentDb);
        }
    }
}