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
        public void GetLecture_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = _client.GetAsync(id).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("6")]
        [TestCase("-6")]
        [TestCase("123")]
        public void GetLecture_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = _client.GetAsync(id).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("")]
        [TestCase(null)]
        public void GetLecture_GivenEmpty_ResponseNotFound(string id)
        {
            // Act
            var response = _client.GetAsync(id).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public void GetLectures_ResponseOk()
        {
            // Act
            var response = _client.GetAsync("lectures").Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public void GetLectures_ReturnLecturesCount_5_FromTestDb()
        {
            // Act
            var response = _client.GetAsync("lectures").Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var lectures = JsonSerializer.Deserialize<LectureDto[]>(res, options);

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

        //[Test]
        //public async Task CreateLecture_GivenNotValidTeacherId_ResponseNotFound()
        //{
        //    // Arrange
        //    var lecture = new LecturePostDto
        //    {
        //        Topic = "New lecture Topic",
        //        Date = System.DateTime.Today,
        //        TeacherId = 5
        //    };

        //    var lectureDto = JsonSerializer.Serialize(lecture);
        //    var content = new StringContent(lectureDto, Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await _client.PostAsync("", content);

        //    // Assert
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        //}

        [TestCase("3")]
        public void EditLecture_GivenValidInfo_ResponseOk(string id)
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
            var response = _client.PutAsync(id, content).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        //[TestCase("3")]
        //public void EditLecture_GivenNotValidInfo_ResponseNotFound(string id)
        //{
        //    // Arrange
        //    var lecture = new LectureDto
        //    {
        //        Id = 3,
        //        Topic = "NChanged lecture Topic",
        //        Date = System.DateTime.Today,
        //        TeacherId = 5
        //    };

        //    var lectureDto = JsonSerializer.Serialize(lecture);
        //    var content = new StringContent(lectureDto, Encoding.UTF8, "application/json");

        //    // Act
        //    var response = _client.PutAsync(id, content).Result;

        //    // Assert
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        //}

        [TestCase("1")]
        [TestCase("5")]
        public void DeleteLecture_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = _client.DeleteAsync(id).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("0")]
        [TestCase("6")]
        public void DeleteLecture_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = _client.DeleteAsync(id).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}