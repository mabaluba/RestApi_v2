// using System;
// using System.Threading.Tasks;
using BenchmarkDotNet.Running;

// using DataAccess;
// using DataAccess.EntityRepositories;
// using M10_RestApi.Tests.IntegrationTests;
// using Microsoft.EntityFrameworkCore;
//
namespace Banch
{
    internal class Program
    {
        // private readonly ILogger<AttendanceRepository> _logger;
        // private readonly IMapper _mapper;
        // public Program()
        // {}
        //
        public static /*async Task*/ void Main()
        {
            BenchmarkRunner.Run<Bench1>();

            // var options = new DbContextOptions<EducationDbContext>();
            // using var context = new EducationDbContext(options);

            // var a = new AttendanceRepository(context, null, null);
            // var i = a.GetEntityAsync(1).Result;
            // Console.WriteLine(i);
        }

        // public static async Task Do1()
        // {
        //    AttendanceControllerIntegrationTests a = new AttendanceControllerIntegrationTests();
        //    await a.GetAllAttendancesAsync_ReturnAttendancesCount_20_FromTestDb();
        // }
    }
}