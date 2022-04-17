using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace M10_RestApi.Tests.IntegrationTests
{
    [TestFixture]
    internal class TeacherControllerIntegrationTests : CustomWebApplicationFactory<Startup>
    {
        private readonly string _url = "/api/education/teacher/";
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
        [TestCase("2")]
        public async Task GetTeacher_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("4")]
        [TestCase("123")]
        public async Task GetTeacher_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("")]
        public async Task GetTeacher_GivenEmpty_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetTeachers_ResponseOk()
        {
            // Act
            var response = await _client.GetAsync("teachers");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetTeachers_ReturnTeachersCount_3_FromTestDb()
        {
            // Act
            var response = await _client.GetAsync("teachers");
            var res = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var teachers = await JsonSerializer.DeserializeAsync<TeacherDto[]>(res, options);

            // Assert
            Assert.That(teachers.Length, Is.EqualTo(3));
        }

        [Test]
        public async Task CreateTeacher_GivenValidTeacherDtoModel_ResponseOk()
        {
            // Arrange
            var teacher = new TeacherPostDto
            {
                FirstName = "Sherlock ",
                LastName = "Holmes",
                Email = "sherlock@holmes.com",
                PhoneNumber = "123-123-1234"
            };

            var teacherDto = JsonSerializer.Serialize(teacher);
            var content = new StringContent(teacherDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("2")]
        public async Task EditTeacher_(string id)
        {
            // Arrange
            var teacher = new TeacherDto
            {
                Id = 2,
                FirstName = "Change Name",
                LastName = "Looney",
                Email = "sharon2@knowledge.com",
                PhoneNumber = "377-555-0132"
            };

            var teacherDto = JsonSerializer.Serialize(teacher);
            var content = new StringContent(teacherDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(id, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task DeleteTeacher_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = await _client.DeleteAsync(id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("4")]
        public async Task DeleteTeacher_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.DeleteAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}