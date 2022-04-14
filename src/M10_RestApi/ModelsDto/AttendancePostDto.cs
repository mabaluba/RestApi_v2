using System.ComponentModel;

namespace M10_RestApi.ModelsDto
{
    public class AttendancePostDto
    {
        public string LectureTopic { get; set; }

        public string StudentFirstName { get; set; }

        public string StudentLastName { get; set; }

        [DefaultValue(false)]
        public bool? IsAttended { get; set; }

        public int? HomeworkMark { get; set; }
    }
}