using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using M10_RestApi.Tests.IntegrationTests;

namespace Banch
{
    [MemoryDiagnoser]
    public class Bench1
    {
        // private readonly AttendanceControllerIntegrationTests _a = new();
        //
        // [Params(10)]
        // public static int IterationCount { get; set; }
        //
        [Benchmark]
        public async Task Do1()
        {
            // for (int i = 0; i < IterationCount; i++)
            // {
                AttendanceControllerIntegrationTests a = new();

                // await a.CreateAttendanceAsync_GivenValidAttendanceDtoModel_ResponseOk();
                //
                await a.EditAttendanceAsync_GivenValidInfo_ResponseOk("15");

            // }
            //
        }
    }
}