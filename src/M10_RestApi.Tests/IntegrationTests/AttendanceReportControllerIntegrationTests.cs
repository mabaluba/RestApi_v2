using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace M10_RestApi.Tests.IntegrationTests
{
    using System.Text;

    [TestFixture]
    internal class AttendanceReportReportControllerIntegrationTests : CustomWebApplicationFactory<Startup>
    {
        private readonly string _url = "/api/education/attendancereport/";
        private readonly string _byLecture = "byLecture/";
        private readonly string _byStudent = "byStudent";
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

        [TestCase("Architecture")]
        public async Task GetAttendencesByTopic_GivenValidTopic_ResponseOk(string topic)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byLecture);

            // Act
            var response = await _client.GetAsync(topic);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("Hydraulics")]
        public async Task GetAttendencesByTopic_GivenValidTopicWithNoAttandances_ResponseNotFound(string topic)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byLecture);

            // Act
            var response = await _client.GetAsync(topic);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("Not Valid")]
        [TestCase("")]
        public async Task GetAttendencesByTopic_GivenNotValidTopic_ResponseOk(string topic)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byLecture);

            // Act
            var response = await _client.GetAsync(topic);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("Architecture")]
        public async Task GetAttendanceReportsByTopic_ReturnCount_5_FromTestDb(string topic)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byLecture);

            // Act
            var response = await _client.GetAsync(topic);
            var res = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var attendanceReports = await JsonSerializer.DeserializeAsync<AttendanceDto[]>(res, options);

            // Assert
            Assert.That(attendanceReports.Length, Is.EqualTo(5));
        }

        [Test]
        public async Task GetAttendencesByStudentName_GivenValidName_ResponseOk()
        {
            // Arrange
            var name = new StudentFirstLastNameDto
            {
                FirstName = "Janet",
                LastName = "Gates"
            };

            var nameDto = JsonSerializer.Serialize(name);
            var content = new StringContent(nameDto, Encoding.UTF8, "application/json");
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url);

            // Act
            var response = await _client.PostAsync(_byStudent, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("Not,Found")]
        public async Task GetAttendencesByStudentName_GivenNotValidName_ResponseNotFound(string name)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byStudent);

            // Act
            var response = await _client.GetAsync(name);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("")]
        [TestCase("   ")]
        public async Task GetAttendencesByStudentName_GivenNotValidName_ResponseMethodNotAllowed(string name)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byStudent);

            // Act
            var response = await _client.GetAsync(name);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [TestCase("Christopher,Beck")]
        public async Task GetAttendencesByStudentName_GivenNotValidNameWithNoAttandances_ResponseNotFound(string name)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byStudent);

            // Act
            var response = await _client.GetAsync(name);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetAttendencesByStudentName_ReturnCount_4_FromTestDb()
        {
            // Arrange
            var name = new StudentFirstLastNameDto
            {
                FirstName = "Katherine",
                LastName = "Harding"
            };

            var nameDto = JsonSerializer.Serialize(name);
            var content = new StringContent(nameDto, Encoding.UTF8, "application/json");
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url);

            // Act
            var response = await _client.PostAsync(_byStudent, content);
            var res = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var attendanceReports = await JsonSerializer.DeserializeAsync<AttendanceDto[]>(res, options);

            // Assert
            Assert.That(attendanceReports.Length, Is.EqualTo(4));
        }
    }
}