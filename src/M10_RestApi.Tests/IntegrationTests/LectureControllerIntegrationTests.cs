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
    internal class LectureControllerIntegrationTests : CustomWebApplicationFactory<Startup>
    {
        private readonly string _url = "/api/education/lecture/";
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
        public async Task GetLectureAsync_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("6")]
        [TestCase("-6")]
        [TestCase("123")]
        public async Task GetLectureAsync_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task GetLectureAsync_GivenEmpty_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetLecturesAsync_ResponseOk()
        {
            // Act
            var response = await _client.GetAsync("lectures");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetLecturesAsync_ReturnLecturesCount_5_FromTestDb()
        {
            // Act
            var response = await _client.GetAsync("lectures");
            var res = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var lectures = await JsonSerializer.DeserializeAsync<LectureDto[]>(res, options);

            // Assert
            Assert.That(lectures.Length, Is.EqualTo(5));
        }

        [Test]
        public async Task CreateLecture_GivenValidLectureDtoModel_ResponseOk()
        {
            // Arrange
            var lecture = new LecturePostDto
            {
                Topic = "New lecture Topic",
                Date = System.DateTime.Today,
                TeacherId = 2
            };

            var lectureDto = JsonSerializer.Serialize(lecture);
            var content = new StringContent(lectureDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task CreateLecture_GivenNotValidLectureDtoModel_ResponseBadRequest()
        {
            // Arrange
            var lecture = new LecturePostDto
            {
                Topic = null,
                Date = System.DateTime.Today,
                TeacherId = 2
            };

            var lectureDto = JsonSerializer.Serialize(lecture);
            var content = new StringContent(lectureDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        // [Test]
        // public async Task CreateLecture_GivenNotValidTeacherId_ResponseNotFound()
        // {
        //    // Arrange
        //    var lecture = new LecturePostDto
        //    {
        //        Topic = "New lecture Topic",
        //        Date = System.DateTime.Today,
        //        TeacherId = 5
        //    };
        //
        //    var lectureDto = JsonSerializer.Serialize(lecture);
        //    var content = new StringContent(lectureDto, Encoding.UTF8, "application/json");
        //
        //    // Act
        //    var response = await _client.PostAsync("", content);
        //
        //    // Assert
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        // }
        //
        [TestCase("3")]
        public async Task EditLectureAsync_GivenValidInfo_ResponseOk(string id)
        {
            // Arrange
            var lecture = new LectureDto
            {
                Id = 3,
                Topic = "NChanged lecture Topic",
                Date = System.DateTime.Today,
                TeacherId = 3
            };

            var lectureDto = JsonSerializer.Serialize(lecture);
            var content = new StringContent(lectureDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(id, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        // [TestCase("3")]
        // public void EditLecture_GivenNotValidInfo_ResponseNotFound(string id)
        // {
        //    // Arrange
        //    var lecture = new LectureDto
        //    {
        //        Id = 3,
        //        Topic = "NChanged lecture Topic",
        //        Date = System.DateTime.Today,
        //        TeacherId = 5
        //    };
        //
        //    var lectureDto = JsonSerializer.Serialize(lecture);
        //    var content = new StringContent(lectureDto, Encoding.UTF8, "application/json");
        //
        //    // Act
        //    var response = _client.PutAsync(id, content).Result;
        //
        //    // Assert
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        // }
        //
        [TestCase("1")]
        [TestCase("5")]
        public async Task DeleteLecture_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = await _client.DeleteAsync(id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("6")]
        public async Task DeleteLecture_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.DeleteAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}