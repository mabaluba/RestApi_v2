using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
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
        public void GetAverageGrade_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = _client.GetAsync(id).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("7")]
        [TestCase("-6")]
        [TestCase("123")]
        [TestCase("")]
        public void GetAverageGrade_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = _client.GetAsync(id).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void GetAverageGrades_ResponseOk()
        {
            // Act
            var response = _client.GetAsync("allstudents").Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public void GetAverageGrades_ReturnStudentsAVG_Count_6_FromTestDb()
        {
            // Act
            var response = _client.GetAsync("allstudents").Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var students = JsonSerializer.Deserialize<AverageGradeDto[]>(res, options);

            // Assert
            Assert.That(students.Length, Is.EqualTo(6));
        }

        [Test]
        public void GetAverageGrades_ReturnCorrectStudentsAVG_FromTestDb()
        {
            // Act
            var response = _client.GetAsync("allstudents").Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var students = JsonSerializer.Deserialize<AverageGradeDto[]>(res, options);
            var grades = students.Select(i => i.StudentAverageGrade);
            var expected = new double[] { 4.25, 3.5, 4.25, 0.5, 0, 0 };

            // Assert
            Assert.That(grades, Is.EquivalentTo(expected));
        }
    }
}