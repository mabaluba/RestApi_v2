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
        public async Task GetStudent_GivenValidId_ResponseOk(string id)
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
        public async Task GetStudent_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("")]
        public async Task GetStudent_GivenEmpty_ResponseMethodNotAllowed(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetStudents_ResponseOk()
        {
            // Act
            var response = await _client.GetAsync("students");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetStudents_ReturnStudentsCount_6_FromTestDb()
        {
            // Act
            StudentDto[] students = await GetStudentsAsync();

            // Assert
            Assert.That(students.Length, Is.EqualTo(6));
        }

        private async Task<StudentDto[]> GetStudentsAsync()
        {
            var response = await _client.GetAsync("students");
            var res = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var students = await JsonSerializer.DeserializeAsync<StudentDto[]>(res, options);
            return students;
        }

        [Test]
        public async Task CreateStudent_GivenValidStudentDtoModel_ResponseOk()
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
            var response = await _client.PostAsync("", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("3")]
        public async Task EditStudent_(string id)
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
            var response = await _client.PutAsync(id, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task DeleteStudent_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = await _client.DeleteAsync(id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("7")]
        public async Task DeleteStudent_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.DeleteAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}