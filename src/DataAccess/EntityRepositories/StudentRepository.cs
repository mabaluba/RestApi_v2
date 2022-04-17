using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAccess.Models;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace DataAccess.EntityRepositories
{
    internal class StudentRepository : IEntityRepository<IStudent>
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

        public IStudent CreateEntity(IStudent student)
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
                result = _context.Students?.Add(studentDb);
                _context.SaveChanges();
                _logger.LogInformation($"Saved member with id = {studentDb.Id} to database.");
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, exception.Message, studentDb);
                throw;
            }

            return _mapper.Map<Student>(result.Entity);
        }

        public IStudent EditEntity(IStudent student)
        {
            var studentDb = _context.Students?.Find(student.Id) ?? throw new MissingMemberException($"Cannot find member with Id = {student.Id}.");

            studentDb.FirstName = student.FirstName;
            studentDb.LastName = student.LastName;
            studentDb.Email = student.Email;
            studentDb.PhoneNumber = student.PhoneNumber;
            try
            {
                _context.Update(studentDb);
                _context.SaveChanges();
                _logger.LogInformation($"Saved changes for member with id = {studentDb.Id} to database.");
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            return _mapper.Map<Student>(studentDb);
        }

        public void DeleteEntity(int studentId)
        {
            var studentDb = _context.Students?.Find(studentId) ?? throw new MissingMemberException($"Cannot find member with Id = {studentId}.");

            _context.Students.Remove(studentDb);
            _context.SaveChanges();
            _logger.LogInformation($"Delete member with id = {studentId} from database.");
        }

        public IReadOnlyCollection<IStudent> GetAllEntities()
        {
            var students = _context.Students?.ToArray() ?? throw new ArgumentNullException();
            _ = students.Length == 0 ? throw new MissingMemberException($"Cannot find any {typeof(Student).Name} members.") : 0;

            _logger.LogInformation($"Get all members {typeof(Student).Name} from database.");
            return _mapper.Map<IReadOnlyCollection<Student>>(students);
        }

        public IStudent GetEntity(int studentId)
        {
            var studentDb = _context.Students?.Find(studentId) ?? throw new MissingMemberException($"Cannot find member with Id = {studentId}.");

            _logger.LogInformation($"Get member with id = {studentId} from database.");
            return _mapper.Map<Student>(studentDb);
        }
    }
}