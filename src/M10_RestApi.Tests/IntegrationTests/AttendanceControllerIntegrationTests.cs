using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using M10_RestApi.ModelsDto;
using NUnit.Framework;

namespace M10_RestApi.Tests.IntegrationTests
{
    [TestFixture]
    internal class AttendanceControllerIntegrationTests : CustomWebApplicationFactory<Startup>
    {
        private readonly string _url = "/api/education/attendance/";
        private HttpClient _client;

        // For Benchmark
        // public AttendanceControllerIntegrationTests()
        // {
        //    _client = new CustomWebApplicationFactory<Startup>().CreateClient();
        //    _client.BaseAddress = new System.Uri(_client.BaseAddress, _url);
        // }
        //
        [SetUp]
        public void SetUp()
        {
            _client = new CustomWebApplicationFactory<Startup>().CreateClient();
            _client.BaseAddress = new System.Uri(_client.BaseAddress, _url);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [TestCase("1")]
        [TestCase("20")]
        public async Task GetAttendanceAsync_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("21")]
        [TestCase("123")]
        public async Task GetAttendanceAsync_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("0")]
        [TestCase("-6")]
        public async Task GetAttendanceAsync_GivenNotValidId_ResponseBadRequest(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("")]
        public async Task GetAttendanceAsync_GivenEmpty_ResponseMethodNotAllowed(string id)
        {
            // Act
            var response = await _client.GetAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetAllAttendancesAsync_ResponseOk()
        {
            // Act
            var response = await _client.GetAsync("attendances");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetAllAttendancesAsync_ReturnAttendancesCount_20_FromTestDb()
        {
            // Arrange
            var count = 20;

            // Act
            var response = await _client.GetAsync("attendances");
            var res = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var attendances = await JsonSerializer.DeserializeAsync<AttendanceDto[]>(res, options);

            // Assert
            Assert.That(attendances.Length, Is.EqualTo(count));
        }

        [Test]
        public async Task CreateAttendanceAsync_GivenValidAttendanceDtoModel_ResponseOk()
        {
            // Arrange
            var attendance = new AttendancePostDto
            {
                LectureTopic = "Hydraulics",
                StudentFirstName = "Janet",
                StudentLastName = "Gates",
                IsAttended = true,
                HomeworkMark = 5
            };

            // Asynchronously serialize from object to string
            // using var stream = new MemoryStream();
            // await JsonSerializer.SerializeAsync(stream, attendance);
            // stream.Position = 0;
            // using var reader = new StreamReader(stream);
            // var attendanceDto = await reader.ReadToEndAsync();
            //
            var attendanceDto = JsonSerializer.Serialize(attendance);
            var content = new StringContent(attendanceDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCaseSource(nameof(_notValidAttendancesPost))]
        public async Task CreateAttendanceAsync_GivenNotValidAttendanceDtoModel_ResponseNotFound(AttendancePostDto attendance)
        {
            // Arrang
            var attendanceDto = JsonSerializer.Serialize(attendance);
            var content = new StringContent(attendanceDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("15")]
        public async Task EditAttendanceAsync_GivenValidInfo_ResponseOk(string id)
        {
            // Arrange
            var attendance = new AttendanceDto
            {
                Id = 15,
                LectureTopic = "Hydraulics",
                StudentFirstName = "Janet",
                StudentLastName = "Gates",
                IsAttended = false,
                HomeworkMark = 0
            };

            var attendanceDto = JsonSerializer.Serialize(attendance);
            var content = new StringContent(attendanceDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(id, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCaseSource(nameof(_notValidAttendances))]
        public async Task EditAttendanceAsync_GivenNotValidInfo_ResponseNotFound(AttendanceDto attendance)
        {
            // Arrange
            var id = "15";
            var attendanceDto = JsonSerializer.Serialize(attendance);
            var content = new StringContent(attendanceDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(id, content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("1")]
        [TestCase("20")]
        public async Task DeleteAttendanceAsync_GivenValidId_ResponseOk(string id)
        {
            // Act
            var response = await _client.DeleteAsync(id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestCase("21")]
        public async Task DeleteAttendanceAsync_GivenNotValidId_ResponseNotFound(string id)
        {
            // Act
            var response = await _client.DeleteAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("0")]
        public async Task DeleteAttendanceAsync_GivenNotValidId_ResponseBadRequest(string id)
        {
            // Act
            var response = await _client.DeleteAsync(id);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        private static AttendancePostDto[] _notValidAttendancesPost =
        {
            new()
            {
                LectureTopic = "Not Valid",
                StudentFirstName = "Janet",
                StudentLastName = "Gates",
                IsAttended = true,
                HomeworkMark = 5
            },
            new()
            {
                LectureTopic = "Hydraulics",
                StudentFirstName = "Not Valid",
                StudentLastName = "Gates",
                IsAttended = true,
                HomeworkMark = 5
            },
            new()
            {
                LectureTopic = "Hydraulics",
                StudentFirstName = "Janet",
                StudentLastName = "Not Valid",
                IsAttended = true,
                HomeworkMark = 5
            },
        };

        private static AttendanceDto[] _notValidAttendances =
        {
            new()
            {
                Id = 15,
                LectureTopic = "Not Valid",
                StudentFirstName = "Janet",
                StudentLastName = "Gates",
                IsAttended = true,
                HomeworkMark = 5
            },
            new()
            {
                Id = 15,
                LectureTopic = "Hydraulics",
                StudentFirstName = "Not Valid",
                StudentLastName = "Gates",
                IsAttended = true,
                HomeworkMark = 5
            },
            new()
            {
                Id = 15,
                LectureTopic = "Hydraulics",
                StudentFirstName = "Janet",
                StudentLastName = "Not Valid",
                IsAttended = true,
                HomeworkMark = 5
            },
        };
    }
}