﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityDomain.DomainEntites;
using UniversityDomain.EntityInterfaces;
using UniversityDomain.ServiceInterfaces;

namespace DataAccess.EntityRepositories
{
    internal class AverageGradeRepositoryAsync : IAverageGradeRepositoryAsync<IAverageGrade>
    {
        private readonly ILogger<AverageGradeRepositoryAsync> _logger;
        private readonly IMapper _mapper;
        private readonly EducationDbContext _context;

        public AverageGradeRepositoryAsync(EducationDbContext context, IMapper mapper, ILogger<AverageGradeRepositoryAsync> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task<IAverageGrade> EditEntityAsync(IAverageGrade student)
        {
            _ = student ?? throw new ArgumentNullException(nameof(student));

            var studentDb = await _context.Students?.FirstOrDefaultAsync(i => i.FirstName == student.FirstName && i.LastName == student.LastName)
                ?? throw new MissingMemberException($"Cannot find member with FirstName: {student.FirstName}, LastName: {student.LastName}.");

            studentDb.StudentAverageGrade = student.StudentAverageGrade;
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

            return _mapper.Map<AverageGrade>(studentDb);
        }

        public async Task<IReadOnlyCollection<IAverageGrade>> GetAllEntitiesAsync()
        {
            var students = await _context.Students.ToArrayAsync() ?? throw new ArgumentNullException();
            _ = students.Length == 0 ? throw new MissingMemberException($"Cannot find any {typeof(Student).Name} members.") : 0;
            _logger.LogInformation($"Get all members {typeof(Student).Name} from database.");
            return _mapper.Map<IReadOnlyCollection<AverageGrade>>(students);
        }

        public async Task<IAverageGrade> GetEntityAsync(int studentId)
        {
            var studentDb = await _context.Students.FindAsync(studentId) ?? throw new MissingMemberException($"Cannot find member with Id = {studentId}.");

            _logger.LogInformation($"Get member with id = {studentId} from database.");
            return _mapper.Map<AverageGrade>(studentDb);
        }
    }
}