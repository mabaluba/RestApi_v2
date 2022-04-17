using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace M10_RestApi.Tests.IntegrationTests
{
    [TestFixture]
    internal class StudentAverageGradeControllerIntegrationTests : CustomWebApplicationFactory<Startup>
    {
        private readonly string _url = "/api/education/averagegrade/";
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new CustomWebApplicationFactory<Startup>()
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = true
                });
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [TestCase("1")]
        [TestCase("6")]
        public async Task GetAverageGrade_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("7")]
        [TestCase("-6")]
        [TestCase("123")]
        [TestCase("")]
        public async Task GetAverageGrade_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetAverageGrades_ResponseOk()
        {
            // Act
            var response = await _client.GetAsync("allstudents");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetAverageGrades_ReturnStudentsAVG_Count_6_FromTestDb()
        {
            // Act
            AverageGradeDto[] students = await GetStudentsAsync();

            // Assert
            Assert.That(students.Length, Is.EqualTo(6));
        }

        [Test]
        public async Task GetAverageGrades_ReturnCorrectStudentsAVG_FromTestDb()
        {
            // Act
            AverageGradeDto[] students = await GetStudentsAsync();
            var grades = students.Select(i => i.StudentAverageGrade);
            var expected = new double[] { 4.25, 3.5, 4.25, 0.5, 0, 0 };

            // Assert
            Assert.That(grades, Is.EquivalentTo(expected));
        }

        private async Task<AverageGradeDto[]> GetStudentsAsync()
        {
            var response = await _client.GetAsync("allstudents");
            var res = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var students = await JsonSerializer.DeserializeAsync<AverageGradeDto[]>(res, options);
            return students;
        }
    }
}