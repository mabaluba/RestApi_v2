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
    internal class LectureRepository : IEntityRepositoryAsync<ILecture>
    {
        private readonly ILogger<LectureRepository> _logger;
        private readonly IMapper _mapper;
        private readonly EducationDbContext _context;

        public LectureRepository(EducationDbContext context, IMapper mapper, ILogger<LectureRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task<ILecture> CreateEntityAsync(ILecture lecture)
        {
            _ = lecture ?? throw new ArgumentNullException(nameof(lecture));

            EntityEntry<LectureDb> result;
            try
            {
                var lectureDb = new LectureDb()
                {
                    Topic = lecture.Topic,
                    Date = lecture.Date,
                    TeacherId = lecture.TeacherId
                };

                result = await _context.Lectures.AddAsync(lectureDb);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Saved member with id = {lectureDb.Id} to database.");
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            return _mapper.Map<Lecture>(result.Entity);
        }

        public async Task<ILecture> EditEntityAsync(ILecture lecture)
        {
            _ = lecture ?? throw new ArgumentNullException(nameof(lecture));

            var lectureDb = await _context.Lectures.FindAsync(lecture.Id)
                ?? throw new MissingMemberException($"Cannot find member with Id = {lecture.Id}.");

            lectureDb.Topic = lecture.Topic;
            lectureDb.Date = lecture.Date;
            lectureDb.TeacherId = lecture.TeacherId;
            try
            {
                _context.Update(lectureDb);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Saved changes for member with id = {lectureDb.Id} to database.");
            }
            catch (Exception exception) when (exception is InvalidOperationException || exception is DbUpdateException)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            return _mapper.Map<Lecture>(lectureDb);
        }

        public async Task DeleteEntityAsync(int lectureId)
        {
            var lectureDb = await _context.Lectures.FindAsync(lectureId)
                ?? throw new MissingMemberException($"Cannot find member with Id = {lectureId}.");

            _context.Lectures.Remove(lectureDb);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Delete member with id = {lectureId} from database.");
        }

        public async Task<IReadOnlyCollection<ILecture>> GetAllEntitiesAsync()
        {
            var lectures = await _context.Lectures?.ToArrayAsync();
            lectures = lectures.Length == 0 ? throw new MissingMemberException($"Cannot find any {typeof(Lecture).Name} members.") : lectures;
            _logger.LogInformation($"Get all members {typeof(Lecture).Name} from database.");
            return _mapper.Map<IReadOnlyCollection<Lecture>>(lectures);
        }

        public async Task<ILecture> GetEntityAsync(int lectureId)
        {
            var lectureDb = await _context.Lectures.FindAsync(lectureId) ?? throw new MissingMemberException($"Cannot find member with Id = {lectureId}.");

            _logger.LogInformation($"Get member with id = {lectureId} from database.");
            return _mapper.Map<Lecture>(lectureDb);
        }
    }
}