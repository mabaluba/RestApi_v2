using System.Net;
using System.Net.Http;
using System.Text.Json;
using M10_RestApi.ModelsDto;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace M10_RestApi.Tests.IntegrationTests
{
    [TestFixture]
    internal class AttendanceReportReportControllerIntegrationTests : CustomWebApplicationFactory<Startup>
    {
        private readonly string _url = "/api/education/attendancereport/";
        private readonly string _byLecture = "byLecture/";
        private readonly string _byStudent = "byStudent/";
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
        public void GetAttendencesByTopic_GivenValidTopic_ResponseOk(string topic)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byLecture);

            // Act
            var response = _client.GetAsync(topic).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("Hydraulics")]
        public void GetAttendencesByTopic_GivenValidTopicWithNoAttandances_ResponseNotFound(string topic)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byLecture);

            // Act
            var response = _client.GetAsync(topic).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("Not Valid")]
        [TestCase("")]
        public void GetAttendencesByTopic_GivenNotValidTopic_ResponseOk(string topic)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byLecture);

            // Act
            var response = _client.GetAsync(topic).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("Architecture")]
        public void GetAttendanceReportsByTopic_ReturnCount_5_FromTestDb(string topic)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byLecture);

            // Act
            var response = _client.GetAsync(topic).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var attendanceReports = JsonSerializer.Deserialize<AttendanceDto[]>(res, options);

            // Assert
            Assert.That(attendanceReports.Length, Is.EqualTo(5));
        }

        [TestCase("Kathleen,Garza")]
        public void GetAttendencesByStudentName_GivenValidName_ResponseOk(string name)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byStudent);

            // Act
            var response = _client.GetAsync(name).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("Not,Valid")]
        [TestCase("")]
        public void GetAttendencesByStudentName_GivenNotValidName_ResponseOk(string name)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byStudent);

            // Act
            var response = _client.GetAsync(name).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("Christopher,Beck")]
        public void GetAttendencesByStudentName_GivenNotValidNameWithNoAttandances_ResponseNotFound(string name)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byStudent);

            // Act
            var response = _client.GetAsync(name).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("Katherine,Harding")]
        public void GetAttendencesByStudentName_ReturnCount_4_FromTestDb(string name)
        {
            // Arrange
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url + _byStudent);

            // Act
            var response = _client.GetAsync(name).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var attendanceReports = JsonSerializer.Deserialize<AttendanceDto[]>(res, options);

            // Assert
            Assert.That(attendanceReports.Length, Is.EqualTo(4));
        }
    }
}