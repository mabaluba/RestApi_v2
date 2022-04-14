using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace M10_RestApi.Tests.IntegrationTests
{
    [TestFixture]
    internal class StudentControllerIntegrationTests : CustomWebApplicationFactory<Startup>
    {
        private readonly string _url = "/api/education/student/";
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
        [TestCase("5")]
        public void GetStudent_GivenValidId_ResponseOk(string id)
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
        public void GetStudent_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = _client.GetAsync(id).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("")]
        public void GetStudent_GivenEmpty_ResponseMethodNotAllowed(string id)
        {
            // Act
            var response = _client.GetAsync(id).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public void GetStudents_ResponseOk()
        {
            // Act
            var response = _client.GetAsync("students").Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public void GetStudents_ReturnStudentsCount_6_FromTestDb()
        {
            // Act
            var response = _client.GetAsync("students").Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var students = JsonSerializer.Deserialize<StudentDto[]>(res, options);

            // Assert
            Assert.That(students.Length, Is.EqualTo(6));
        }

        [Test]
        public void CreateStudent_GivenValidStudentDtoModel_ResponseOk()
        {
            // Arrange
            var student = new StudentPostDto
            {
                FirstName = "Sherlock ",
                LastName = "Holmes",
                Email = "sherlock@holmes.com",
                PhoneNumber = "123-123-1234"
            };

            var studentDto = JsonSerializer.Serialize(student);
            var content = new StringContent(studentDto, Encoding.UTF8, "application/json");

            // Act
            var response = _client.PostAsync("", content).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("3")]
        public void EditStudent_(string id)
        {
            // Arrange
            var student = new StudentDto
            {
                Id = 3,
                FirstName = "Change Name",
                LastName = "Looney",
                Email = "sharon2@knowledge.com",
                PhoneNumber = "377-555-0132"
            };

            var studentDto = JsonSerializer.Serialize(student);
            var content = new StringContent(studentDto, Encoding.UTF8, "application/json");

            // Act
            var response = _client.PutAsync(id, content).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("1")]
        [TestCase("2")]
        public void DeleteStudent_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = _client.DeleteAsync(id).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("7")]
        public void DeleteStudent_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = _client.DeleteAsync(id).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}