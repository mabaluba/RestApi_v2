using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public sealed class EducationDbContext : DbContext
    {
        public EducationDbContext(DbContextOptions<EducationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<LectureDb>()
            //    .HasOne<TeacherDb>()
            //    .WithMany()
            //    .HasForeignKey(l => l.TeacherId);
            //
            modelBuilder.Entity<LectureDb>()
                .HasIndex(l => l.Topic).IsUnique();

            modelBuilder.Entity<AttendanceDb>()
                .HasIndex(a => new { a.LectureId, a.StudentId }).IsUnique();

            modelBuilder.Entity<TeacherDb>()
                .HasIndex(t => new { t.FirstName, t.LastName }).IsUnique();

            modelBuilder.Entity<StudentDb>()
                .HasIndex(t => new { t.FirstName, t.LastName }).IsUnique();
        }

        internal DbSet<StudentDb> Students { get; set; }

        internal DbSet<TeacherDb> Teachers { get; set; }

        internal DbSet<LectureDb> Lectures { get; set; }

        internal DbSet<AttendanceDb> Attendances { get; set; }
    }
}